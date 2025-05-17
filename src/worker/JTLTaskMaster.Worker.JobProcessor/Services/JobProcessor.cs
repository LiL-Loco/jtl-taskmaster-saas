using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Worker.JobProcessor;

public class JobProcessingOptions
{
    public TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(30);
}

public class TaskExecutionException : Exception
{
    public Guid TaskId { get; }

    public TaskExecutionException(Guid taskId, string message) : base(message)
    {
        TaskId = taskId;
    }
}

public class RetryStrategy
{
    public int MaxRetries { get; }
    public TimeSpan BaseDelay { get; }

    public RetryStrategy(int maxRetries, TimeSpan baseDelay)
    {
        MaxRetries = maxRetries;
        BaseDelay = baseDelay;
    }

    public TimeSpan GetDelay(int retryCount)
    {
        // Exponential backoff: baseDelay * 2^retryCount
        return TimeSpan.FromTicks(BaseDelay.Ticks * (long)Math.Pow(2, retryCount));
    }
}

public class JobProcessor : BackgroundService
{
    private readonly ILogger<JobProcessor> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly JobProcessingOptions _options;

    public JobProcessor(
        ILogger<JobProcessor> logger,
        IServiceProvider serviceProvider,
        IOptions<JobProcessingOptions> options)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingJobs(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing jobs");
            }

            await Task.Delay(_options.PollingInterval, stoppingToken);
        }
    }

    private async Task ProcessPendingJobs(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
        var jobs = await jobService.GetPendingJobsAsync(stoppingToken);

        foreach (var job in jobs)
        {
            if (stoppingToken.IsCancellationRequested) break;

            try
            {
                await ProcessJob(job, scope, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing job {JobId}", job.Id);
                await jobService.MarkJobAsFailed(job.Id, ex.Message);
            }
        }
    }

    private async Task ProcessJob(Job job, IServiceScope scope, CancellationToken stoppingToken)
    {
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();
        var agentService = scope.ServiceProvider.GetRequiredService<IAgentService>();

        await jobService.MarkJobAsRunning(job.Id);

        foreach (var task in job.Tasks.OrderBy(t => t.Order))
        {
            if (stoppingToken.IsCancellationRequested) break;

            try
            {
                await ProcessTask(task, agentService, stoppingToken);
            }
            catch (Exception ex)
            {
                await HandleTaskError(task, ex, jobService);
                throw; // Re-throw to mark job as failed
            }
        }

        await jobService.MarkJobAsCompleted(job.Id);
    }

    private async Task ProcessTask(JobTask task, IAgentService agentService, CancellationToken stoppingToken)
    {
        var agent = await agentService.GetAvailableAgentForTask(task);
        if (agent == null)
        {
            throw new InvalidOperationException($"No agent available for task type: {task.Type}");
        }

        var result = await agentService.ExecuteTaskAsync(agent.Id, task, stoppingToken);
        if (!result.Success)
        {
            throw new TaskExecutionException(task.Id, result.ErrorMessage ?? "Unknown error");
        }
    }

    private async Task HandleTaskError(JobTask task, Exception ex, IJobService jobService)
    {
        var retryStrategy = GetRetryStrategy(task.Type);
        var shouldRetry = task.RetryCount < retryStrategy.MaxRetries;

        if (shouldRetry)
        {
            await jobService.MarkTaskForRetry(
                task.Id,
                task.RetryCount + 1,
                DateTime.UtcNow.Add(retryStrategy.GetDelay(task.RetryCount))
            );
        }
        else
        {
            await jobService.MarkTaskAsFailed(task.Id, ex.Message);
        }
    }

    private RetryStrategy GetRetryStrategy(string taskType) => taskType switch
    {
        "jtl-ameise-import" => new RetryStrategy(maxRetries: 3, baseDelay: TimeSpan.FromMinutes(5)),
        "versanddaten-import" => new RetryStrategy(maxRetries: 5, baseDelay: TimeSpan.FromMinutes(2)),
        "ftp-download" => new RetryStrategy(maxRetries: 3, baseDelay: TimeSpan.FromMinutes(1)),
        _ => new RetryStrategy(maxRetries: 2, baseDelay: TimeSpan.FromMinutes(1))
    };
}
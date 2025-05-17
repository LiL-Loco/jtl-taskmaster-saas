using JTLTaskMaster.Agent.Config;
using JTLTaskMaster.Agent.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Agent.Services
{
    public class AgentService : BackgroundService
    {
        private readonly ILogger<AgentService> _logger;
        private readonly AgentConfig _config;
        private readonly ITaskExecutorFactory _taskExecutorFactory;
        private readonly HubConnection _hubConnection;
        private readonly ConcurrentDictionary<string, TaskExecutionContext> _runningTasks;
        private Timer? _heartbeatTimer;

        public AgentService(
            ILogger<AgentService> logger,
            IOptions<AgentConfig> config,
            ITaskExecutorFactory taskExecutorFactory)
        {
            _logger = logger;
            _config = config.Value;
            _taskExecutorFactory = taskExecutorFactory;
            _runningTasks = new ConcurrentDictionary<string, TaskExecutionContext>();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_config.HubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(_config.ApiKey);
                })
                .WithAutomaticReconnect(new CustomRetryPolicy())
                .Build();

            _hubConnection.On<TaskExecutionRequest>("ExecuteTask", HandleTaskExecution);
            _hubConnection.On<string>("CancelTask", HandleTaskCancellation);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ConnectToHub(stoppingToken);

            _heartbeatTimer = new Timer(
                SendHeartbeat,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            // Keep the service alive until stopping is requested
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _heartbeatTimer?.Change(Timeout.Infinite, 0);
            await _hubConnection.StopAsync(cancellationToken);

            // Cancel all running tasks
            foreach (var context in _runningTasks.Values)
            {
                try
                {
                    context.CancellationTokenSource.Cancel();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error cancelling task {TaskId}", context.TaskId);
                }
            }

            await base.StopAsync(cancellationToken);
        }

        private async Task ConnectToHub(CancellationToken cancellationToken)
        {
            try
            {
                await _hubConnection.StartAsync(cancellationToken);
                await _hubConnection.InvokeAsync("RegisterAgent",
                    _config.AgentId,
                    Environment.MachineName,
                    _config.SupportedTaskTypes,
                    cancellationToken);

                _logger.LogInformation("Connected to hub successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error connecting to hub");
                throw;
            }
        }

        private async Task HandleTaskExecution(TaskExecutionRequest request)
        {
            var context = new TaskExecutionContext(request.TaskId);
            if (!_runningTasks.TryAdd(request.TaskId, context))
            {
                await SendTaskStatus(request.TaskId, "Failed", "Task already running");
                return;
            }

            try
            {
                var executor = _taskExecutorFactory.CreateExecutor(request.Type);
                await SendTaskStatus(request.TaskId, "Running");

                await executor.ExecuteAsync(
                    request.Parameters,
                    new Progress<int>(p => ReportProgress(request.TaskId, p)),
                    context.CancellationToken);

                await SendTaskStatus(request.TaskId, "Completed");
            }
            catch (OperationCanceledException)
            {
                await SendTaskStatus(request.TaskId, "Cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing task {TaskId}", request.TaskId);
                await SendTaskStatus(request.TaskId, "Failed", ex.Message);
            }
            finally
            {
                _runningTasks.TryRemove(request.TaskId, out _);
                context.Dispose();
            }
        }

        private async Task HandleTaskCancellation(string taskId)
        {
            if (_runningTasks.TryGetValue(taskId, out var context))
            {
                context.CancellationTokenSource.Cancel();
                await SendTaskStatus(taskId, "Cancelling");
            }
        }

        private async void SendHeartbeat(object? state)
        {
            try
            {
                var stats = new AgentStats
                {
                    CpuUsage = GetCpuUsage(),
                    MemoryUsage = GetMemoryUsage(),
                    RunningTasks = _runningTasks.Count
                };

                await _hubConnection.InvokeAsync("Heartbeat", _config.AgentId, stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending heartbeat");
            }
        }

        private async Task SendTaskStatus(string taskId, string status, string? message = null)
        {
            try
            {
                await _hubConnection.InvokeAsync("UpdateTaskStatus", taskId, status, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending task status update");
            }
        }

        private void ReportProgress(string taskId, int progress)
        {
            try
            {
                _hubConnection.InvokeAsync("UpdateTaskProgress", taskId, progress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reporting progress");
            }
        }

        private static double GetCpuUsage()
        {
            // Implementierung für CPU-Auslastung
            return 0.0;
        }

        private static double GetMemoryUsage()
        {
            // Implementierung für Speicherauslastung
            return 0.0;
        }
    }

    public class TaskExecutionContext : IDisposable
    {
        public string TaskId { get; }
        public CancellationTokenSource CancellationTokenSource { get; }
        public CancellationToken CancellationToken => CancellationTokenSource.Token;

        public TaskExecutionContext(string taskId)
        {
            TaskId = taskId;
            CancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            CancellationTokenSource.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
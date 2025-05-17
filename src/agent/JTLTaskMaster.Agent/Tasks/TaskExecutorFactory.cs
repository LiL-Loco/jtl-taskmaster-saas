using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using JTLTaskMaster.Shared.Interfaces;
using System;

namespace JTLTaskMaster.Agent.Tasks;

public class TaskExecutorFactory : ITaskExecutorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TaskExecutorFactory> _logger;

    public TaskExecutorFactory(
        IServiceProvider serviceProvider,
        ILogger<TaskExecutorFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public ITaskExecutor CreateExecutor(string type)
    {
        _logger.LogInformation("Creating executor for task type: {TaskType}", type);
        
        return type switch
        {
            // Add your task executors here based on your task types
            // For example:
            // "JtlAmeise" => _serviceProvider.GetRequiredService<JtlAmeiseExecutor>(),
            // "JtlWawiExtern" => _serviceProvider.GetRequiredService<JtlWawiExternExecutor>(),
            _ => throw new NotSupportedException($"Task type not supported: {type}")
        };
    }
}
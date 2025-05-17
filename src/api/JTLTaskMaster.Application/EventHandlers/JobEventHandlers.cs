using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Application.Common.Models;
using JTLTaskMaster.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.EventHandlers;

public class JobCreatedEventHandler : INotificationHandler<DomainEventNotification<JobCreatedEvent>>
{
    private readonly ILogger<JobCreatedEventHandler> _logger;
    private readonly IJobNotificationService _notificationService;

    public JobCreatedEventHandler(
        ILogger<JobCreatedEventHandler> logger,
        IJobNotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task Handle(DomainEventNotification<JobCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        await _notificationService.NotifyJobStatusChanged(domainEvent.Job.Id, domainEvent.Job.Status.ToString());
    }
}

public class JobStartedEventHandler : INotificationHandler<DomainEventNotification<JobStartedEvent>>
{
    private readonly ILogger<JobStartedEventHandler> _logger;
    private readonly IJobNotificationService _notificationService;
    private readonly IJobProcessor _jobProcessor;

    public JobStartedEventHandler(
        ILogger<JobStartedEventHandler> logger,
        IJobNotificationService notificationService,
        IJobProcessor jobProcessor)
    {
        _logger = logger;
        _notificationService = notificationService;
        _jobProcessor = jobProcessor;
    }

    public async Task Handle(DomainEventNotification<JobStartedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        await _notificationService.NotifyJobStatusChanged(domainEvent.Job.Id, domainEvent.Job.Status.ToString());
        
        // Queue job for processing
        await _jobProcessor.EnqueueJob(domainEvent.Job.Id);
    }
}

public class TaskStatusChangedEventHandler : INotificationHandler<DomainEventNotification<TaskStatusChangedEvent>>
{
    private readonly ILogger<TaskStatusChangedEventHandler> _logger;
    private readonly IJobNotificationService _notificationService;

    public TaskStatusChangedEventHandler(
        ILogger<TaskStatusChangedEventHandler> logger,
        IJobNotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task Handle(DomainEventNotification<TaskStatusChangedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        _logger.LogInformation("Domain Event: {DomainEvent}", domainEvent.GetType().Name);

        await _notificationService.NotifyTaskStatusChanged(
            domainEvent.Task.JobId,
            domainEvent.Task.Id,
            domainEvent.NewStatus);
    }
}
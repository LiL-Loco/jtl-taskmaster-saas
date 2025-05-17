using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Domain.Common;
using MediatR;

namespace JTLTaskMaster.Infrastructure.Services;

public class DomainEventService : IDomainEventService
{
    private readonly IPublisher _mediator;
    private readonly ILogger<DomainEventService> _logger;

    public DomainEventService(
        IPublisher mediator,
        ILogger<DomainEventService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Publish(DomainEvent domainEvent)
    {
        _logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
        await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
    {
        return (INotification)Activator.CreateInstance(
            typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
    }
}
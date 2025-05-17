using JTLTaskMaster.Domain.Common;

namespace JTLTaskMaster.Application.Common.Interfaces;

public interface IDomainEventService
{
    Task PublishAsync(DomainEvent domainEvent);
}
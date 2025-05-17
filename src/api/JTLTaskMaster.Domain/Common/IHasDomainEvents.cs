// src/api/JTLTaskMaster.Domain/Common/IHasDomainEvents.cs
namespace JTLTaskMaster.Domain.Common;

public interface IHasDomainEvents
{
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    void AddDomainEvent(DomainEvent domainEvent);
    void RemoveDomainEvent(DomainEvent domainEvent);
    void ClearDomainEvents();
}

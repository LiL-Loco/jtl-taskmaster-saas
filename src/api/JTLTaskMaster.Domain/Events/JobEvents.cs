using JTLTaskMaster.Domain.Common;
using JTLTaskMaster.Domain.Entities;

namespace JTLTaskMaster.Domain.Events;

public class JobCreatedEvent : DomainEvent
{
    public Job Job { get; }

    public JobCreatedEvent(Job job)
    {
        Job = job;
    }
}

public class JobStartedEvent : DomainEvent
{
    public Job Job { get; }

    public JobStartedEvent(Job job)
    {
        Job = job;
    }
}

public class JobCompletedEvent : DomainEvent
{
    public Job Job { get; }

    public JobCompletedEvent(Job job)
    {
        Job = job;
    }
}

public class JobFailedEvent : DomainEvent
{
    public Job Job { get; }
    public string ErrorMessage { get; }

    public JobFailedEvent(Job job, string errorMessage)
    {
        Job = job;
        ErrorMessage = errorMessage;
    }
}

public class TaskStatusChangedEvent : DomainEvent
{
    public JobTask Task { get; }
    public string OldStatus { get; }
    public string NewStatus { get; }

    public TaskStatusChangedEvent(JobTask task, string oldStatus, string newStatus)
    {
        Task = task;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}
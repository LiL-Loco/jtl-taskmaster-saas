namespace JTLTaskMaster.Domain.Exceptions;

public class TaskExecutionException : Exception
{
    public Guid TaskId { get; }

    public TaskExecutionException(Guid taskId, string message)
        : base(message)
    {
        TaskId = taskId;
    }
}

public class ConcurrencyException : Exception
{
    public ConcurrencyException(string message) : base(message)
    {
    }
}
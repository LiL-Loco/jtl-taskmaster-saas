namespace JTLTaskMaster.Agent.Tasks.JtlWawiExternDll;

public class TaskExecutionException : Exception
{
    public TaskExecutionException(string message) : base(message)
    {
    }

    public TaskExecutionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
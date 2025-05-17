using System;

namespace JTLTaskMaster.Agent.Exceptions;

public class TaskExecutionException : Exception
{
    public TaskExecutionException() : base() { }

    public TaskExecutionException(string message) : base(message) { }

    public TaskExecutionException(string message, Exception innerException)
        : base(message, innerException) { }
}
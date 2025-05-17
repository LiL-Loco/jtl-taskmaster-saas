// src/api/JTLTaskMaster.Domain/Exceptions/NotFoundException.cs
namespace JTLTaskMaster.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}

namespace JTLTaskMaster.Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string? UserName { get; }
    Guid TenantId { get; }
    bool IsAuthenticated { get; }
}
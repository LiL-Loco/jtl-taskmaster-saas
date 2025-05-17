using JTLTaskMaster.Domain.Common;
using JTLTaskMaster.Domain.Enums;

namespace JTLTaskMaster.Domain.Entities;

public class Job : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Pending;
    public DateTime? LastRun { get; set; }
    public List<JobTask> Tasks { get; set; } = new();
    public Guid TenantId { get; set; }
}
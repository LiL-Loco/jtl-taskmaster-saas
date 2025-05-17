// src/api/JTLTaskMaster.Domain/Common/BaseAuditableEntity.cs
namespace JTLTaskMaster.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity
    {
        public bool IsDeleted { get; set; }
        public Guid TenantId { get; set; }
    }
}
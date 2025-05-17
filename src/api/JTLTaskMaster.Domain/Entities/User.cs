// src/api/JTLTaskMaster.Domain/Entities/User.cs
using JTLTaskMaster.Domain.Common;

namespace JTLTaskMaster.Domain.Entities
{
    public class User : BaseAuditableEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
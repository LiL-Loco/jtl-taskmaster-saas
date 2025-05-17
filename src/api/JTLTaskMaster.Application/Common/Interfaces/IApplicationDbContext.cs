// src/api/JTLTaskMaster.Application/Common/Interfaces/IApplicationDbContext.cs
using JTLTaskMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Job> Jobs { get; set; }
    DbSet<JobTask> JobTasks { get; }
    DbSet<User> Users { get; } // Und andere benötigte Entities
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

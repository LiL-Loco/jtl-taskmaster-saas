using JTLTaskMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JTLTaskMaster.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Job> Jobs { get; }
        DbSet<JobTask> JobTasks { get; }
        DbSet<User> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
// src/api/JTLTaskMaster.Persistence/Repositories/JobRepository.cs
using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JTLTaskMaster.Persistence.Repositories;

public class JobRepository : IJobRepository
{
    private readonly IApplicationDbContext _context;

    public JobRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Include(j => j.Tasks)
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
    }

    public async Task<List<Job>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Include(j => j.Tasks)
            .ToListAsync(cancellationToken);
    }

    public async Task<Job> AddAsync(Job job, CancellationToken cancellationToken = default)
    {
        await _context.Jobs.AddAsync(job, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return job;
    }

    public async Task UpdateAsync(Job job, CancellationToken cancellationToken = default)
    {
        _context.Jobs.Update(job);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Job job, CancellationToken cancellationToken = default)
    {
        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Job>> GetPendingJobsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Jobs
            .Include(j => j.Tasks)
            .Where(j => j.IsEnabled && j.Status == (Domain.Entities.JobStatus)Domain.Enums.JobStatus.Pending)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkJobAsRunningAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        var job = await _context.Jobs.FindAsync(new object[] { jobId }, cancellationToken);
        if (job != null)
        {
            job.Status = (Domain.Entities.JobStatus)Domain.Enums.JobStatus.Running;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task MarkJobAsCompletedAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        var job = await _context.Jobs.FindAsync(new object[] { jobId }, cancellationToken);
        if (job != null)
        {
            job.Status = (Domain.Entities.JobStatus)Domain.Enums.JobStatus.Completed;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task MarkJobAsFailedAsync(Guid jobId, string error, CancellationToken cancellationToken = default)
    {
        var job = await _context.Jobs.FindAsync(new object[] { jobId }, cancellationToken);
        if (job != null)
        {
            job.Status = (Domain.Entities.JobStatus)Domain.Enums.JobStatus.Failed;
            job.LastError = error;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

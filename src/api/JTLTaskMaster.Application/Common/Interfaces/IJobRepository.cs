// src/api/JTLTaskMaster.Application/Common/Interfaces/IJobRepository.cs
using JTLTaskMaster.Domain.Entities;

namespace JTLTaskMaster.Application.Common.Interfaces;

public interface IJobRepository
{
    Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Job>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Job> AddAsync(Job job, CancellationToken cancellationToken = default);
    Task UpdateAsync(Job job, CancellationToken cancellationToken = default);
    Task DeleteAsync(Job job, CancellationToken cancellationToken = default);
    Task<List<Job>> GetPendingJobsAsync(CancellationToken cancellationToken = default);
    Task MarkJobAsRunningAsync(Guid jobId, CancellationToken cancellationToken = default);
    Task MarkJobAsCompletedAsync(Guid jobId, CancellationToken cancellationToken = default);
    Task MarkJobAsFailedAsync(Guid jobId, string error, CancellationToken cancellationToken = default);
}

using JTLTaskMaster.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.Common.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<Job>> GetPendingJobsAsync(CancellationToken cancellationToken);
        Task MarkJobAsRunning(Guid jobId);
        Task MarkJobAsCompleted(Guid jobId);
        Task MarkJobAsFailed(Guid jobId, string errorMessage);
        Task MarkTaskForRetry(Guid taskId, int retryCount, DateTime nextRetryTime);
        Task MarkTaskAsFailed(Guid taskId, string errorMessage);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Shared.Interfaces;

public interface ITaskExecutor
{
    Task<bool> ExecuteAsync(string parameters, IProgress<int>? progress = null, CancellationToken cancellationToken = default);
}
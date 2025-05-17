namespace JTLTaskMaster.Agent.Tasks;

public interface ITaskExecutor
{
    Task ExecuteAsync(string parameters, IProgress<int>? progress, CancellationToken cancellationToken);
}
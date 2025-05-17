using JTLTaskMaster.Shared.Interfaces;

namespace JTLTaskMaster.Agent.Tasks;

public interface ITaskExecutorFactory
{
    ITaskExecutor CreateExecutor(string type);
}
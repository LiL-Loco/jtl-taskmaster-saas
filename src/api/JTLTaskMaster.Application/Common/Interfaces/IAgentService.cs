using JTLTaskMaster.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.Common.Interfaces
{
    public interface IAgentService
    {
        Task<AgentExecutionResult> ExecuteTaskAsync(string agentId, JobTask task, CancellationToken cancellationToken);
    }

    public class TaskExecutionResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
    public class AgentExecutionResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
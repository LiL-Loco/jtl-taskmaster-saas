namespace JTLTaskMaster.Agent.Tasks;

public class TaskExecutionRequest
{
    public string TaskId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Parameters { get; set; } = string.Empty;
}
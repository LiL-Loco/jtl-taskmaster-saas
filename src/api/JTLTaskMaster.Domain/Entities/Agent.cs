namespace JTLTaskMaster.Domain.Entities;

public class Agent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public string Status { get; set; } = "Available";
    public List<string> SupportedTaskTypes { get; set; } = new();
    public DateTime LastHeartbeat { get; set; }
    public Guid TenantId { get; set; }
}
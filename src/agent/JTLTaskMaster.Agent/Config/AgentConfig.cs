using System;

namespace JTLTaskMaster.Agent.Config;

public class AgentConfig
{
    public string AgentId { get; set; } = string.Empty;
    public string HubUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string[] SupportedTaskTypes { get; set; } = Array.Empty<string>();
}
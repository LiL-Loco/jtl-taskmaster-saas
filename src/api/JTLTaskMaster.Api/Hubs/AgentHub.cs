using Microsoft.AspNetCore.SignalR;
using JTLTaskMaster.Application.Common.Interfaces;

namespace JTLTaskMaster.Api.Hubs;

public class AgentHub : Hub
{
    private readonly IAgentService _agentService;

    public AgentHub(IAgentService agentService)
    {
        _agentService = agentService;
    }

    public async Task RegisterAgent(string agentId, string machineId)
    {
        await _agentService.RegisterAgentConnection(agentId, machineId, Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, agentId);
    }

    public async Task UpdateTaskStatus(string taskId, string status, string? message)
    {
        await _agentService.UpdateTaskStatus(taskId, status, message);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _agentService.HandleAgentDisconnection(Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}
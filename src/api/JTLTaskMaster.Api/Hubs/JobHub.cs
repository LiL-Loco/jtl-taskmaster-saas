using Microsoft.AspNetCore.SignalR;
using JTLTaskMaster.Application.Common.Interfaces;

namespace JTLTaskMaster.Api.Hubs;

public class JobHub : Hub
{
    private readonly ICurrentUserService _currentUserService;

    public JobHub(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override async Task OnConnectedAsync()
    {
        // Add to tenant group for multi-tenancy
        await Groups.AddToGroupAsync(Context.ConnectionId, _currentUserService.TenantId.ToString());
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, _currentUserService.TenantId.ToString());
        await base.OnDisconnectedAsync(exception);
    }
}
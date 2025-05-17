using Microsoft.AspNetCore.SignalR;
using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Api.Hubs;

namespace JTLTaskMaster.Api.Services;

public class JobNotificationService : IJobNotificationService
{
    private readonly IHubContext<JobHub> _hubContext;
    private readonly ICurrentUserService _currentUserService;

    public JobNotificationService(
        IHubContext<JobHub> hubContext,
        ICurrentUserService currentUserService)
    {
        _hubContext = hubContext;
        _currentUserService = currentUserService;
    }

    public async Task NotifyJobStatusChanged(Guid jobId, string status)
    {
        await _hubContext.Clients
            .Group(_currentUserService.TenantId.ToString())
            .SendAsync("JobStatusUpdated", jobId, status);
    }

    public async Task NotifyTaskStatusChanged(Guid jobId, Guid taskId, string status)
    {
        await _hubContext.Clients
            .Group(_currentUserService.TenantId.ToString())
            .SendAsync("TaskStatusUpdated", jobId, taskId, status);
    }
}
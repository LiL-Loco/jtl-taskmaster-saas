// src/api/JTLTaskMaster.Application/Common/Interfaces/IJobNotificationService.cs
namespace JTLTaskMaster.Application.Common.Interfaces;

public interface IJobNotificationService
{
    Task NotifyJobStatusChanged(Guid jobId, string status);
    Task NotifyTaskStatusChanged(Guid jobId, Guid taskId, string status);
}
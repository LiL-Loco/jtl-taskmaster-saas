using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.Features.Jobs.Commands.UpdateJob;

public class UpdateJobCommand : MediatR.IRequest<MediatR.Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public List<UpdateJobTaskDto> Tasks { get; set; } = new();
}

public class UpdateJobTaskDto
{
    public Guid? Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Parameters { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsEnabled { get; set; }
}

public class UpdateJobCommandHandler : MediatR.IRequestHandler<UpdateJobCommand, MediatR.Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateJobCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<MediatR.Unit> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _context.Jobs
            .Include(j => j.Tasks)
            .FirstOrDefaultAsync(j => j.Id == request.Id && j.TenantId == _currentUserService.TenantId, cancellationToken);

        if (job == null)
        {
            throw new Exception($"Job with ID {request.Id} not found");
        }

        job.Name = request.Name;
        job.Description = request.Description;
        job.IsEnabled = request.IsEnabled;
        job.LastModified = DateTime.UtcNow;

        // Remove tasks that are not in the update request
        var taskIdsToKeep = request.Tasks
            .Where(t => t.Id.HasValue)
            .Select(t => t.Id!.Value)
            .ToList();

        var tasksToRemove = job.Tasks
            .Where(t => !taskIdsToKeep.Contains(t.Id))
            .ToList();

        foreach (var task in tasksToRemove)
        {
            job.Tasks.Remove(task);
        }

        // Update existing tasks and add new ones
        foreach (var taskDto in request.Tasks)
        {
            if (taskDto.Id.HasValue)
            {
                // Update existing task
                var existingTask = job.Tasks.FirstOrDefault(t => t.Id == taskDto.Id.Value);
                if (existingTask != null)
                {
                    existingTask.Type = taskDto.Type;
                    existingTask.Parameters = taskDto.Parameters;
                    existingTask.Order = taskDto.Order;
                    existingTask.IsEnabled = taskDto.IsEnabled;
                    existingTask.LastModified = DateTime.UtcNow;
                }
            }
            else
            {
                // Add new task
                job.Tasks.Add(new JobTask
                {
                    Id = Guid.NewGuid(),
                    JobId = job.Id,
                    Type = taskDto.Type,
                    Parameters = taskDto.Parameters,
                    Order = taskDto.Order,
                    IsEnabled = taskDto.IsEnabled,
                    Status = Domain.Enums.TaskStatus.Pending,
                    TenantId = _currentUserService.TenantId,
                    Created = DateTime.UtcNow
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return MediatR.Unit.Value;
    }
}


using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Domain.Entities;
using JTLTaskMaster.Domain.Events;
using JTLTaskMaster.Domain.Enums;

namespace JTLTaskMaster.Application.Features.Jobs.Commands.CreateJob;

public class CreateJobCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public List<CreateJobTaskDto> Tasks { get; set; } = new();
}

public class CreateJobTaskDto
{
    public string Type { get; set; } = string.Empty;
    public string Parameters { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsEnabled { get; set; }
}

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateJobCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateJobCommand request, CancellationToken cancellationToken)
    {
        var entity = new Job
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            IsEnabled = request.IsEnabled,
            TenantId = _currentUserService.TenantId,
            Status = JobStatus.Pending
        };

        var tasks = request.Tasks.Select(t => new JobTask
        {
            Id = Guid.NewGuid(),
            JobId = entity.Id,
            Type = t.Type,
            Parameters = t.Parameters,
            Order = t.Order,
            IsEnabled = t.IsEnabled,
            TenantId = _currentUserService.TenantId,
            Status = JTLTaskMaster.Domain.Enums.TaskStatus.Pending
        }).ToList();

        entity.Tasks.Clear();
        foreach (var task in tasks)
        {
            entity.Tasks.Add(task);
        }
        entity.AddDomainEvent(new JobCreatedEvent(entity));

        await _context.Jobs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
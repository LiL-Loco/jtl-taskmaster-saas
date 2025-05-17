using AutoMapper;
using AutoMapper.QueryableExtensions;

using JTLTaskMaster.Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using JTLTaskMaster.Domain.Entities;
namespace JTLTaskMaster.Application.Features.Jobs.Queries.GetJobs
{
    public class GetJobsQuery : IRequest<List<JobDto>>
    {
    }
}
public record GetJobsQuery : MediatR.IRequest<List<JobDto>>;

public class GetJobsQueryHandler : MediatR.IRequestHandler<GetJobsQuery, List<JobDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetJobsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<List<JobDto>> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Jobs
            .Where(j => j.TenantId == _currentUserService.TenantId)
            .OrderByDescending(j => j.Created)
            .ProjectTo<JobDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}

public class JobDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsEnabled { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? LastRun { get; set; }
    public List<JobTaskDto> Tasks { get; set; } = new();
}

public class JobTaskDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Parameters { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsEnabled { get; set; }
    public string? Status { get; set; }
}

public interface IApplicationDbContext
{
    DbSet<Job> Jobs { get; } // Add this line
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
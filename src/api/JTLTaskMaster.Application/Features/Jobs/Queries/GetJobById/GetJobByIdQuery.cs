using AutoMapper;
using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Application.Features.Jobs.Queries.GetJobs;
using Microsoft.EntityFrameworkCore;

namespace JTLTaskMaster.Application.Features.Jobs.Queries.GetJobById;

public class GetJobByIdQuery : IRequest<JobDto?>
{
    public Guid Id { get; set; }
}

public class GetJobByIdQueryHandler : IRequestHandler<GetJobByIdQuery, JobDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public GetJobByIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<JobDto?> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        var job = await _context.Jobs
            .Include(j => j.Tasks)
            .FirstOrDefaultAsync(j => j.Id == request.Id && j.TenantId == _currentUserService.TenantId, cancellationToken);

        return job != null ? _mapper.Map<JobDto>(job) : null;
    }
}
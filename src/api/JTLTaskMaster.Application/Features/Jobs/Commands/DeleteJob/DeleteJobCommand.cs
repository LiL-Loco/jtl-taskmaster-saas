using JTLTaskMaster.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.Features.Jobs.Commands.DeleteJob;

public class DeleteJobCommand : MediatR.IRequest<MediatR.Unit>
{
    public Guid Id { get; set; }
}

public class DeleteJobCommandHandler : MediatR.IRequestHandler<DeleteJobCommand, MediatR.Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteJobCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<MediatR.Unit> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _context.Jobs
            .Include(j => j.Tasks)
            .FirstOrDefaultAsync(j => j.Id == request.Id && j.TenantId == _currentUserService.TenantId, cancellationToken);

        if (job == null)
        {
            throw new Exception($"Job with ID {request.Id} not found");
        }

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync(cancellationToken);

        return MediatR.Unit.Value;
    }
}
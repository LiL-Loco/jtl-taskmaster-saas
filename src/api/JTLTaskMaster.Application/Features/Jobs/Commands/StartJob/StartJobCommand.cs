// src/api/JTLTaskMaster.Application/Features/Jobs/Commands/StartJob/StartJobCommand.cs
using JTLTaskMaster.Domain.Exceptions;
using JTLTaskMaster.Domain.Entities;
using JTLTaskMaster.Domain.Enums;
using MediatR;

namespace JTLTaskMaster.Application.Features.Jobs.Commands.StartJob;

public record StartJobCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
}

public class StartJobCommandHandler : IRequestHandler<StartJobCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public StartJobCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(StartJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _context.Jobs.FindAsync(new object[] { request.Id }, cancellationToken);

        if (job == null)
            throw new NotFoundException(nameof(Job), request.Id);

        job.Status = JobStatus.Running;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

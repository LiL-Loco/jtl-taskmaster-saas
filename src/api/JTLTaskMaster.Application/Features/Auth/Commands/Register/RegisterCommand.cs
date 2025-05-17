using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Application.Common.Models;
using JTLTaskMaster.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.Features.Auth.Commands.Register;

public record RegisterCommand : MediatR.IRequest<Result>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}

public class RegisterCommandHandler : MediatR.IRequestHandler<RegisterCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var (result, userId) = await _identityService.CreateUserAsync(request.Email, request.Password);

        if (!result.Succeeded)
        {
            return result;
        }

        var user = new User
        {
            Id = userId,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true,
            TenantId = Guid.NewGuid(), // In real app, this would come from tenant creation process
            Roles = new List<string> { "User" }
        };

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
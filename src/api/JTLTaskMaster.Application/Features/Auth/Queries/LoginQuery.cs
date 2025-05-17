using JTLTaskMaster.Application.Common.Exceptions;
using JTLTaskMaster.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JTLTaskMaster.Application.Features.Auth.Queries;

public record LoginQuery : MediatR.IRequest<LoginResponse>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}

public class LoginQueryHandler : MediatR.IRequestHandler<LoginQuery, LoginResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public LoginQueryHandler(IApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid credentials");
        }

        var token = await _tokenService.CreateTokenAsync(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        return new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddDays(7)
        };
    }
}
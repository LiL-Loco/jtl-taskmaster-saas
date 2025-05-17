using JTLTaskMaster.Application.Common.Interfaces;
using JTLTaskMaster.Domain.Exceptions;
using JTLTaskMaster.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using static BCrypt.Net.BCrypt;

namespace JTLTaskMaster.Application.Features.Auth.Queries;

public record LoginQuery : MediatR.IRequest<LoginResponse> // Explicitly using MediatR interface
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

public class LoginQueryHandler : MediatR.IRequestHandler<LoginQuery, LoginResponse> // Explicitly using MediatR interface
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

        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.Email); // Spezifischere Exception
        }

        if (!Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidPasswordException(); // Spezifischere Exception
        }

        var token = await _tokenService.CreateTokenAsync(user);
        var refreshToken = _tokenService.GenerateRefreshToken(); // Using the synchronous method from the interface

        return new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddDays(7)
        };
    }
}

// Beispiel für die neuen Exceptions
public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base("Invalid password.") { }
}

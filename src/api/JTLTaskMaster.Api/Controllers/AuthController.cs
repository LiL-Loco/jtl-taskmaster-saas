using Microsoft.AspNetCore.Mvc;
using JTLTaskMaster.Application.Features.Auth.Commands;
using JTLTaskMaster.Application.Features.Auth.Queries;

namespace JTLTaskMaster.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResult>> Register(RegisterCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResult>> Login(LoginQuery query)
    {
        return await _mediator.Send(query);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthenticationResult>> RefreshToken([FromBody] string refreshToken)
    {
        return await _mediator.Send(new RefreshTokenCommand { RefreshToken = refreshToken });
    }
}
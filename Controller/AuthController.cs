using FirstProject.Application.Common;
using FirstProject.Application.Features.Auth.Commands.Login;
using FirstProject.Application.Features.Auth.Commands.RefreshToken;
using FirstProject.Application.Features.Auth.Queries.GetCurrentUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("ping")]
    [AllowAnonymous]
    public IActionResult Ping() => Ok(new { message = "Auth API OK", route = "api/Auth" });

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<Result<LoginResponse>>> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            return Unauthorized(result);
        return Ok(result);
    }

    [HttpPost("refresh")]
    [Authorize]
    public async Task<ActionResult<Result<RefreshTokenResponse>>> Refresh()
    {
        var result = await _mediator.Send(new RefreshTokenCommand());
        if (!result.IsSuccess)
            return Unauthorized(result);
        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<Result<CurrentUserDto>>> GetCurrentUser()
    {
        var result = await _mediator.Send(new GetCurrentUserQuery());
        if (!result.IsSuccess)
            return Unauthorized(result);
        return Ok(result);
    }
}

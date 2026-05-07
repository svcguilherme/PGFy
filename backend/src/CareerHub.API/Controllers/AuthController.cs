using CareerHub.Application.Identity.Commands.Login;
using CareerHub.Application.Identity.Commands.RefreshToken;
using CareerHub.Application.Identity.Commands.Register;
using CareerHub.Application.Identity.Commands.RevokeToken;
using CareerHub.Application.Identity.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub.API.Controllers;

[Route("api/v1/auth")]
public class AuthController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await mediator.Send(new RegisterCommand(dto));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await mediator.Send(new LoginCommand(dto));
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenDto dto)
    {
        var result = await mediator.Send(new RefreshTokenCommand(dto));
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(result.Error);
    }

    [HttpPost("revoke"), Authorize]
    public async Task<IActionResult> Revoke()
    {
        var result = await mediator.Send(new RevokeTokenCommand(UserId));
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}

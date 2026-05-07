using CareerHub.Application.Identity.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Identity.Commands.Login;

public class LoginHandler(IAuthService authService) : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    public Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken ct)
        => authService.LoginAsync(request.Dto, ct);
}

using CareerHub.Application.Identity.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Identity.Commands.RefreshToken;

public class RefreshTokenHandler(IAuthService authService) : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    public Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken ct)
        => authService.RefreshTokenAsync(request.Dto, ct);
}

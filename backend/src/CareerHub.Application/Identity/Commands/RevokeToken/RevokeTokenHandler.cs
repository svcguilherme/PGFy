using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Identity.Commands.RevokeToken;

public class RevokeTokenHandler(IAuthService authService) : IRequestHandler<RevokeTokenCommand, Result<bool>>
{
    public Task<Result<bool>> Handle(RevokeTokenCommand request, CancellationToken ct)
        => authService.RevogarTokensAsync(request.UsuarioId, ct);
}

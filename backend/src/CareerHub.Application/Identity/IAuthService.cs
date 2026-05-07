using CareerHub.Application.Identity.DTOs;
using CareerHub.SharedKernel;

namespace CareerHub.Application.Identity;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> RegistrarAsync(RegisterDto dto, CancellationToken ct = default);
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto, CancellationToken ct = default);
    Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto, CancellationToken ct = default);
    Task<Result<bool>> RevogarTokensAsync(Guid usuarioId, CancellationToken ct = default);
}

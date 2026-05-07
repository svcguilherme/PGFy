using System.Security.Claims;
using CareerHub.Application.Identity;
using CareerHub.Application.Identity.DTOs;
using CareerHub.Infrastructure.Persistence;
using CareerHub.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CareerHub.Infrastructure.Identity;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService,
    AppDbContext context) : IAuthService
{
    public async Task<Result<AuthResponseDto>> RegistrarAsync(RegisterDto dto, CancellationToken ct = default)
    {
        if (await userManager.FindByEmailAsync(dto.Email) is not null)
            return Result.Failure<AuthResponseDto>("E-mail já cadastrado.");

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            NomeCompleto = dto.NomeCompleto
        };

        var identityResult = await userManager.CreateAsync(user, dto.Password);
        if (!identityResult.Succeeded)
        {
            var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            return Result.Failure<AuthResponseDto>(errors);
        }

        await userManager.AddToRoleAsync(user, "User");
        return await GerarTokenResponseAsync(user, ct);
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
            return Result.Failure<AuthResponseDto>("E-mail ou senha inválidos.");

        return await GerarTokenResponseAsync(user, ct);
    }

    public async Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto, CancellationToken ct = default)
    {
        var principal = tokenService.ValidarTokenExpirado(dto.AccessToken);
        if (principal is null)
            return Result.Failure<AuthResponseDto>("Access token inválido.");

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId!);
        if (user is null)
            return Result.Failure<AuthResponseDto>("Usuário não encontrado.");

        var refreshToken = await context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken
                                   && r.UsuarioId == user.Id
                                   && !r.Revogado
                                   && r.Expiracao > DateTime.UtcNow, ct);

        if (refreshToken is null)
            return Result.Failure<AuthResponseDto>("Refresh token inválido ou expirado.");

        refreshToken.Revogado = true;
        await context.SaveChangesAsync(ct);
        return await GerarTokenResponseAsync(user, ct);
    }

    public async Task<Result<bool>> RevogarTokensAsync(Guid usuarioId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(usuarioId.ToString());
        if (user is null)
            return Result.Failure<bool>("Usuário não encontrado.");

        await context.RefreshTokens
            .Where(r => r.UsuarioId == usuarioId && !r.Revogado)
            .ExecuteUpdateAsync(s => s.SetProperty(r => r.Revogado, true), ct);

        return Result.Success(true);
    }

    private async Task<Result<AuthResponseDto>> GerarTokenResponseAsync(ApplicationUser user, CancellationToken ct)
    {
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.GerarAccessToken(user, roles);
        var (refreshTokenStr, expiracao) = tokenService.GerarRefreshToken();

        context.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshTokenStr,
            Expiracao = expiracao,
            UsuarioId = user.Id
        });
        await context.SaveChangesAsync(ct);

        return Result.Success(new AuthResponseDto(accessToken, refreshTokenStr));
    }
}

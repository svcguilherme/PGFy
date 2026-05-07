using System.Security.Claims;

namespace CareerHub.Infrastructure.Identity;

public interface ITokenService
{
    string GerarAccessToken(ApplicationUser user, IList<string> roles);
    (string Token, DateTime Expiracao) GerarRefreshToken();
    ClaimsPrincipal? ValidarTokenExpirado(string token);
}

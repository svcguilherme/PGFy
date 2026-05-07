---
name: auth-patterns
description: Use this skill when working with authentication, authorization, JWT tokens, refresh tokens, ASP.NET Core Identity, user registration, login, roles, or any security-related feature. Trigger on: "jwt", "login", "register", "autenticação", "autorização", "role", "identity", "refresh token".
---

# Auth Patterns — CareerHub

## Estrutura Identity

```csharp
// Infrastructure/Identity/ApplicationUser.cs
public class ApplicationUser : IdentityUser<Guid>
{
    public string NomeCompleto { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public List<RefreshToken> RefreshTokens { get; set; } = [];
}

// Infrastructure/Identity/RefreshToken.cs
public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracao { get; set; }
    public bool Revogado { get; set; }
    public Guid UsuarioId { get; set; }
}
```

## JWT Configuration (appsettings.json)

```json
{
  "JwtSettings": {
    "SecretKey": "$(JWT_SECRET_KEY)",
    "Issuer": "CareerHub",
    "Audience": "CareerHubUsers",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

## Token Service Interface + Implementação

```csharp
public interface ITokenService
{
    string GerarAccessToken(ApplicationUser user, IList<string> roles);
    string GerarRefreshToken();
    ClaimsPrincipal? ValidarTokenExpirado(string token);
}

// Implementação em Infrastructure/Identity/TokenService.cs
public class TokenService(IOptions<JwtSettings> settings) : ITokenService
{
    public string GerarAccessToken(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.NomeCompleto),
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Value.SecretKey));
        var token = new JwtSecurityToken(
            issuer: settings.Value.Issuer,
            audience: settings.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(settings.Value.AccessTokenExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

## Auth Controller Pattern

```csharp
[ApiController, Route("api/v1/auth")]
public class AuthController(IMediator mediator) : ControllerBase
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
        await mediator.Send(new RevokeTokenCommand(UserId));
        return NoContent();
    }
}
```

## Helper para UserId no Controller

```csharp
// Adicionar em ControllerBase extension ou base controller
protected Guid UserId =>
    Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
```

## Program.cs — Registrar Identity + JWT

```csharp
// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(opts => {
    opts.Password.RequireDigit = true;
    opts.Password.RequiredLength = 8;
    opts.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts => {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
            ValidateIssuer = true, ValidIssuer = "CareerHub",
            ValidateAudience = true, ValidAudience = "CareerHubUsers",
            ValidateLifetime = true, ClockSkew = TimeSpan.Zero
        };
    });
```

## Roles
- `Admin` → acesso total, gerencia usuários
- `User` → acesso somente aos próprios dados (filtrar sempre por `UsuarioId`)

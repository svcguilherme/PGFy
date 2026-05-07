namespace CareerHub.Infrastructure.Identity;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracao { get; set; }
    public bool Revogado { get; set; }
    public Guid UsuarioId { get; set; }
}

using CareerHub.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerHub.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Token).IsRequired();
        builder.HasOne<ApplicationUser>()
               .WithMany(u => u.RefreshTokens)
               .HasForeignKey(r => r.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

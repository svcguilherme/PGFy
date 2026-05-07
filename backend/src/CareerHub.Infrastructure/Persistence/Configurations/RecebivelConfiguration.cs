using CareerHub.Domain.Financeiro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerHub.Infrastructure.Persistence.Configurations;

public class RecebivelConfiguration : IEntityTypeConfiguration<Recebivel>
{
    public void Configure(EntityTypeBuilder<Recebivel> builder)
    {
        builder.ToTable("recebiveis");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Descricao).HasMaxLength(300).IsRequired();
        builder.Property(r => r.ValorPrevisto).HasColumnType("numeric(18,2)").IsRequired();

        builder.HasIndex(r => r.UsuarioId);
        builder.HasIndex(r => new { r.UsuarioId, r.DataPrevista });
    }
}

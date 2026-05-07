using CareerHub.Domain.Estudos.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerHub.Infrastructure.Persistence.Configurations;

public class EstudoConfiguration : IEntityTypeConfiguration<Estudo>
{
    public void Configure(EntityTypeBuilder<Estudo> builder)
    {
        builder.ToTable("estudos");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Titulo).HasMaxLength(200).IsRequired();
        builder.Property(e => e.DiaDaSemana).HasConversion<string>().IsRequired();
        builder.Property(e => e.Descricao).HasMaxLength(1000);

        builder.Ignore(e => e.HorasTotais);

        builder.HasIndex(e => new { e.UsuarioId, e.DiaDaSemana });
    }
}

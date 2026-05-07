using CareerHub.Domain.Financeiro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerHub.Infrastructure.Persistence.Configurations;

public class DespesaConfiguration : IEntityTypeConfiguration<Despesa>
{
    public void Configure(EntityTypeBuilder<Despesa> builder)
    {
        builder.ToTable("despesas");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Descricao).HasMaxLength(300).IsRequired();
        builder.Property(d => d.Valor).HasColumnType("numeric(18,2)").IsRequired();
        builder.Property(d => d.Categoria).HasConversion<string>().IsRequired();

        builder.HasIndex(d => d.UsuarioId);
        builder.HasIndex(d => new { d.UsuarioId, d.DataPrevista });
    }
}

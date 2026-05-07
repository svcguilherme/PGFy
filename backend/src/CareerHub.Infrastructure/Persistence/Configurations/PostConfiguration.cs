using CareerHub.Domain.Posts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerHub.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Titulo).HasMaxLength(300).IsRequired();
        builder.Property(p => p.Conteudo).IsRequired();
        builder.Property(p => p.DataPublicacao).IsRequired();

        builder.HasIndex(p => p.UsuarioId);
    }
}

using CareerHub.Domain.Estudos.Entities;
using CareerHub.Domain.Financeiro.Entities;
using CareerHub.Domain.Posts.Entities;
using CareerHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareerHub.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Estudo> Estudos => Set<Estudo>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Despesa> Despesas => Set<Despesa>();
    public DbSet<Recebivel> Recebiveis => Set<Recebivel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

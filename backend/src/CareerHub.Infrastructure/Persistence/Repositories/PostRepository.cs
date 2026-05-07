using CareerHub.Domain.Posts.Entities;
using CareerHub.Domain.Posts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerHub.Infrastructure.Persistence.Repositories;

public class PostRepository(AppDbContext context) : IPostRepository
{
    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Posts.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IEnumerable<Post>> GetByUsuarioAsync(Guid usuarioId, CancellationToken ct = default)
        => await context.Posts
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.DataPublicacao)
            .ToListAsync(ct);

    public async Task AddAsync(Post post, CancellationToken ct = default)
    {
        await context.Posts.AddAsync(post, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Post post, CancellationToken ct = default)
    {
        context.Posts.Update(post);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var post = await context.Posts.FindAsync([id], ct);
        if (post is not null)
        {
            context.Posts.Remove(post);
            await context.SaveChangesAsync(ct);
        }
    }
}

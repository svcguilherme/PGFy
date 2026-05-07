using CareerHub.Domain.Posts.Entities;

namespace CareerHub.Domain.Posts.Interfaces;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Post>> GetByUsuarioAsync(Guid usuarioId, CancellationToken ct = default);
    Task AddAsync(Post post, CancellationToken ct = default);
    Task UpdateAsync(Post post, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

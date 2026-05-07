using CareerHub.Domain.Estudos.Entities;

namespace CareerHub.Domain.Estudos.Interfaces;

public interface IEstudoRepository
{
    Task<Estudo?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Estudo>> GetByUsuarioAsync(Guid usuarioId, CancellationToken ct = default);
    Task<IEnumerable<Estudo>> GetByDiaAsync(Guid usuarioId, DiaDaSemana dia, CancellationToken ct = default);
    Task AddAsync(Estudo estudo, CancellationToken ct = default);
    Task UpdateAsync(Estudo estudo, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

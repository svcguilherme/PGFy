using CareerHub.Domain.Financeiro.Entities;

namespace CareerHub.Domain.Financeiro.Interfaces;

public interface IRecebivelRepository
{
    Task<Recebivel?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Recebivel>> GetByUsuarioAsync(Guid usuarioId, CancellationToken ct = default);
    Task<IEnumerable<Recebivel>> GetByMesAsync(Guid usuarioId, int ano, int mes, CancellationToken ct = default);
    Task AddAsync(Recebivel recebivel, CancellationToken ct = default);
    Task UpdateAsync(Recebivel recebivel, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

using CareerHub.Domain.Financeiro.Entities;

namespace CareerHub.Domain.Financeiro.Interfaces;

public interface IDespesaRepository
{
    Task<Despesa?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Despesa>> GetByUsuarioAsync(Guid usuarioId, CancellationToken ct = default);
    Task<IEnumerable<Despesa>> GetByMesAsync(Guid usuarioId, int ano, int mes, CancellationToken ct = default);
    Task AddAsync(Despesa despesa, CancellationToken ct = default);
    Task UpdateAsync(Despesa despesa, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

using CareerHub.Domain.Financeiro.Entities;
using CareerHub.Domain.Financeiro.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerHub.Infrastructure.Persistence.Repositories;

public class DespesaRepository(AppDbContext context) : IDespesaRepository
{
    public async Task<Despesa?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Despesas.FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<IEnumerable<Despesa>> GetByUsuarioAsync(Guid usuarioId, CancellationToken ct = default)
        => await context.Despesas
            .Where(d => d.UsuarioId == usuarioId)
            .OrderByDescending(d => d.DataPrevista)
            .ToListAsync(ct);

    public async Task<IEnumerable<Despesa>> GetByMesAsync(Guid usuarioId, int ano, int mes, CancellationToken ct = default)
        => await context.Despesas
            .Where(d => d.UsuarioId == usuarioId && d.DataPrevista.Year == ano && d.DataPrevista.Month == mes)
            .OrderBy(d => d.DataPrevista)
            .ToListAsync(ct);

    public async Task AddAsync(Despesa despesa, CancellationToken ct = default)
    {
        await context.Despesas.AddAsync(despesa, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Despesa despesa, CancellationToken ct = default)
    {
        context.Despesas.Update(despesa);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var despesa = await context.Despesas.FindAsync([id], ct);
        if (despesa is not null)
        {
            context.Despesas.Remove(despesa);
            await context.SaveChangesAsync(ct);
        }
    }
}

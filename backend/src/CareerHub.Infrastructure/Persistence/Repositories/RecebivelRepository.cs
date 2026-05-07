using CareerHub.Domain.Financeiro.Entities;
using CareerHub.Domain.Financeiro.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerHub.Infrastructure.Persistence.Repositories;

public class RecebivelRepository(AppDbContext context) : IRecebivelRepository
{
    public async Task<Recebivel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Recebiveis.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<IEnumerable<Recebivel>> GetByUsuarioAsync(Guid usuarioId, CancellationToken ct = default)
        => await context.Recebiveis
            .Where(r => r.UsuarioId == usuarioId)
            .OrderByDescending(r => r.DataPrevista)
            .ToListAsync(ct);

    public async Task<IEnumerable<Recebivel>> GetByMesAsync(Guid usuarioId, int ano, int mes, CancellationToken ct = default)
        => await context.Recebiveis
            .Where(r => r.UsuarioId == usuarioId && r.DataPrevista.Year == ano && r.DataPrevista.Month == mes)
            .OrderBy(r => r.DataPrevista)
            .ToListAsync(ct);

    public async Task AddAsync(Recebivel recebivel, CancellationToken ct = default)
    {
        await context.Recebiveis.AddAsync(recebivel, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Recebivel recebivel, CancellationToken ct = default)
    {
        context.Recebiveis.Update(recebivel);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var recebivel = await context.Recebiveis.FindAsync([id], ct);
        if (recebivel is not null)
        {
            context.Recebiveis.Remove(recebivel);
            await context.SaveChangesAsync(ct);
        }
    }
}

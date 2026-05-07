using CareerHub.Domain.Estudos;
using CareerHub.Domain.Estudos.Entities;
using CareerHub.Domain.Estudos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareerHub.Infrastructure.Persistence.Repositories;

public class EstudoRepository(AppDbContext context) : IEstudoRepository
{
    public async Task<Estudo?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Estudos.FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IEnumerable<Estudo>> GetByUsuarioAsync(Guid usuarioId, CancellationToken ct = default)
        => await context.Estudos
            .Where(e => e.UsuarioId == usuarioId)
            .OrderBy(e => e.DiaDaSemana).ThenBy(e => e.HoraInicio)
            .ToListAsync(ct);

    public async Task<IEnumerable<Estudo>> GetByDiaAsync(Guid usuarioId, DiaDaSemana dia, CancellationToken ct = default)
        => await context.Estudos
            .Where(e => e.UsuarioId == usuarioId && e.DiaDaSemana == dia)
            .OrderBy(e => e.HoraInicio)
            .ToListAsync(ct);

    public async Task AddAsync(Estudo estudo, CancellationToken ct = default)
    {
        await context.Estudos.AddAsync(estudo, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Estudo estudo, CancellationToken ct = default)
    {
        context.Estudos.Update(estudo);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var estudo = await context.Estudos.FindAsync([id], ct);
        if (estudo is not null)
        {
            context.Estudos.Remove(estudo);
            await context.SaveChangesAsync(ct);
        }
    }
}

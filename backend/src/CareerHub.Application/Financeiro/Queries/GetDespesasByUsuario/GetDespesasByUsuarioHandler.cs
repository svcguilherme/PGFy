using AutoMapper;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Queries.GetDespesasByUsuario;

public class GetDespesasByUsuarioHandler(IDespesaRepository repo, IMapper mapper)
    : IRequestHandler<GetDespesasByUsuarioQuery, Result<IEnumerable<DespesaDto>>>
{
    public async Task<Result<IEnumerable<DespesaDto>>> Handle(GetDespesasByUsuarioQuery query, CancellationToken ct)
    {
        var despesas = await repo.GetByUsuarioAsync(query.UsuarioId, ct);
        return Result.Success(mapper.Map<IEnumerable<DespesaDto>>(despesas));
    }
}

using AutoMapper;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Queries.GetReceiveisByUsuario;

public class GetReceiveisByUsuarioHandler(IRecebivelRepository repo, IMapper mapper)
    : IRequestHandler<GetReceiveisByUsuarioQuery, Result<IEnumerable<RecebivelDto>>>
{
    public async Task<Result<IEnumerable<RecebivelDto>>> Handle(GetReceiveisByUsuarioQuery query, CancellationToken ct)
    {
        var recebiveis = await repo.GetByUsuarioAsync(query.UsuarioId, ct);
        return Result.Success(mapper.Map<IEnumerable<RecebivelDto>>(recebiveis));
    }
}

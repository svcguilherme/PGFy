using AutoMapper;
using CareerHub.Application.Estudos.DTOs;
using CareerHub.Domain.Estudos.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Estudos.Queries.GetEstudosByDia;

public class GetEstudosByDiaHandler(IEstudoRepository repo, IMapper mapper)
    : IRequestHandler<GetEstudosByDiaQuery, Result<IEnumerable<EstudoDto>>>
{
    public async Task<Result<IEnumerable<EstudoDto>>> Handle(GetEstudosByDiaQuery query, CancellationToken ct)
    {
        var estudos = await repo.GetByDiaAsync(query.UsuarioId, query.DiaDaSemana, ct);
        return Result.Success(mapper.Map<IEnumerable<EstudoDto>>(estudos));
    }
}

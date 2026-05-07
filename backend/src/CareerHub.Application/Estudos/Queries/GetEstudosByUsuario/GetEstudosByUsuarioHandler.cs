using AutoMapper;
using CareerHub.Application.Estudos.DTOs;
using CareerHub.Domain.Estudos.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Estudos.Queries.GetEstudosByUsuario;

public class GetEstudosByUsuarioHandler(IEstudoRepository repo, IMapper mapper)
    : IRequestHandler<GetEstudosByUsuarioQuery, Result<IEnumerable<EstudoDto>>>
{
    public async Task<Result<IEnumerable<EstudoDto>>> Handle(GetEstudosByUsuarioQuery query, CancellationToken ct)
    {
        var estudos = await repo.GetByUsuarioAsync(query.UsuarioId, ct);
        return Result.Success(mapper.Map<IEnumerable<EstudoDto>>(estudos));
    }
}

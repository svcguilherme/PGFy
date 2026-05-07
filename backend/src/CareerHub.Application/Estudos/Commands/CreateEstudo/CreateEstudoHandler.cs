using AutoMapper;
using CareerHub.Application.Estudos.DTOs;
using CareerHub.Domain.Estudos.Entities;
using CareerHub.Domain.Estudos.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Estudos.Commands.CreateEstudo;

public class CreateEstudoHandler(IEstudoRepository repo, IMapper mapper)
    : IRequestHandler<CreateEstudoCommand, Result<EstudoDto>>
{
    public async Task<Result<EstudoDto>> Handle(CreateEstudoCommand cmd, CancellationToken ct)
    {
        var result = Estudo.Create(
            cmd.Dto.Titulo, cmd.Dto.DiaDaSemana,
            cmd.Dto.HoraInicio, cmd.Dto.HoraFim,
            cmd.UsuarioId, cmd.Dto.Descricao);

        if (!result.IsSuccess)
            return Result.Failure<EstudoDto>(result.Error!);

        await repo.AddAsync(result.Value!, ct);
        return Result.Success(mapper.Map<EstudoDto>(result.Value!));
    }
}

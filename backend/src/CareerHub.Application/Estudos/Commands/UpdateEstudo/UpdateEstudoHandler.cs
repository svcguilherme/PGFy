using AutoMapper;
using CareerHub.Application.Estudos.DTOs;
using CareerHub.Domain.Estudos.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Estudos.Commands.UpdateEstudo;

public class UpdateEstudoHandler(IEstudoRepository repo, IMapper mapper)
    : IRequestHandler<UpdateEstudoCommand, Result<EstudoDto>>
{
    public async Task<Result<EstudoDto>> Handle(UpdateEstudoCommand cmd, CancellationToken ct)
    {
        var estudo = await repo.GetByIdAsync(cmd.EstudoId, ct);
        if (estudo is null)
            return Result.Failure<EstudoDto>("Estudo não encontrado.");

        if (estudo.UsuarioId != cmd.UsuarioId)
            return Result.Failure<EstudoDto>("Acesso negado.");

        var update = estudo.Atualizar(
            cmd.Dto.Titulo, cmd.Dto.DiaDaSemana,
            cmd.Dto.HoraInicio, cmd.Dto.HoraFim,
            cmd.Dto.Descricao);

        if (!update.IsSuccess)
            return Result.Failure<EstudoDto>(update.Error!);

        await repo.UpdateAsync(estudo, ct);
        return Result.Success(mapper.Map<EstudoDto>(estudo));
    }
}

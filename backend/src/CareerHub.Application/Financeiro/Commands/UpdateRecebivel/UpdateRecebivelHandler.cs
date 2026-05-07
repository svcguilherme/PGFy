using AutoMapper;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.UpdateRecebivel;

public class UpdateRecebivelHandler(IRecebivelRepository repo, IMapper mapper)
    : IRequestHandler<UpdateRecebivelCommand, Result<RecebivelDto>>
{
    public async Task<Result<RecebivelDto>> Handle(UpdateRecebivelCommand cmd, CancellationToken ct)
    {
        var recebivel = await repo.GetByIdAsync(cmd.RecebivelId, ct);
        if (recebivel is null)
            return Result.Failure<RecebivelDto>("Recebível não encontrado.");

        if (recebivel.UsuarioId != cmd.UsuarioId)
            return Result.Failure<RecebivelDto>("Acesso negado.");

        var update = recebivel.Atualizar(cmd.Dto.Descricao, cmd.Dto.ValorPrevisto, cmd.Dto.DataPrevista);
        if (!update.IsSuccess)
            return Result.Failure<RecebivelDto>(update.Error!);

        await repo.UpdateAsync(recebivel, ct);
        return Result.Success(mapper.Map<RecebivelDto>(recebivel));
    }
}

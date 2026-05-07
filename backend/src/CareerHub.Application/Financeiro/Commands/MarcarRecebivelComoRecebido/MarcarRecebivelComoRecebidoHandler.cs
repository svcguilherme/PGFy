using AutoMapper;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.MarcarRecebivelComoRecebido;

public class MarcarRecebivelComoRecebidoHandler(IRecebivelRepository repo, IMapper mapper)
    : IRequestHandler<MarcarRecebivelComoRecebidoCommand, Result<RecebivelDto>>
{
    public async Task<Result<RecebivelDto>> Handle(MarcarRecebivelComoRecebidoCommand cmd, CancellationToken ct)
    {
        var recebivel = await repo.GetByIdAsync(cmd.RecebivelId, ct);
        if (recebivel is null)
            return Result.Failure<RecebivelDto>("Recebível não encontrado.");

        if (recebivel.UsuarioId != cmd.UsuarioId)
            return Result.Failure<RecebivelDto>("Acesso negado.");

        recebivel.MarcarComoRecebido();
        await repo.UpdateAsync(recebivel, ct);
        return Result.Success(mapper.Map<RecebivelDto>(recebivel));
    }
}

using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.DeleteRecebivel;

public class DeleteRecebivelHandler(IRecebivelRepository repo)
    : IRequestHandler<DeleteRecebivelCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteRecebivelCommand cmd, CancellationToken ct)
    {
        var recebivel = await repo.GetByIdAsync(cmd.RecebivelId, ct);
        if (recebivel is null)
            return Result.Failure<bool>("Recebível não encontrado.");

        if (recebivel.UsuarioId != cmd.UsuarioId)
            return Result.Failure<bool>("Acesso negado.");

        await repo.DeleteAsync(cmd.RecebivelId, ct);
        return Result.Success(true);
    }
}

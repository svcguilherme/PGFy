using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.DeleteDespesa;

public class DeleteDespesaHandler(IDespesaRepository repo)
    : IRequestHandler<DeleteDespesaCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteDespesaCommand cmd, CancellationToken ct)
    {
        var despesa = await repo.GetByIdAsync(cmd.DespesaId, ct);
        if (despesa is null)
            return Result.Failure<bool>("Despesa não encontrada.");

        if (despesa.UsuarioId != cmd.UsuarioId)
            return Result.Failure<bool>("Acesso negado.");

        await repo.DeleteAsync(cmd.DespesaId, ct);
        return Result.Success(true);
    }
}

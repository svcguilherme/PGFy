using CareerHub.Domain.Estudos.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Estudos.Commands.DeleteEstudo;

public class DeleteEstudoHandler(IEstudoRepository repo)
    : IRequestHandler<DeleteEstudoCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteEstudoCommand cmd, CancellationToken ct)
    {
        var estudo = await repo.GetByIdAsync(cmd.EstudoId, ct);
        if (estudo is null)
            return Result.Failure<bool>("Estudo não encontrado.");

        if (estudo.UsuarioId != cmd.UsuarioId)
            return Result.Failure<bool>("Acesso negado.");

        await repo.DeleteAsync(cmd.EstudoId, ct);
        return Result.Success(true);
    }
}

using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.DeleteDespesa;

public record DeleteDespesaCommand(Guid DespesaId, Guid UsuarioId) : IRequest<Result<bool>>;

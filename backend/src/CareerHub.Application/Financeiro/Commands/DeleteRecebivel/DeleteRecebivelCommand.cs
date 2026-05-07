using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.DeleteRecebivel;

public record DeleteRecebivelCommand(Guid RecebivelId, Guid UsuarioId) : IRequest<Result<bool>>;

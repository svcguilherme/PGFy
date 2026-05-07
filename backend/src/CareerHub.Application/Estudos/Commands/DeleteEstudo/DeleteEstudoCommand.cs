using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Estudos.Commands.DeleteEstudo;

public record DeleteEstudoCommand(Guid EstudoId, Guid UsuarioId) : IRequest<Result<bool>>;

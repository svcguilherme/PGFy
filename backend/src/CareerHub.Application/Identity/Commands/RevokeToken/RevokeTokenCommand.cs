using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Identity.Commands.RevokeToken;

public record RevokeTokenCommand(Guid UsuarioId) : IRequest<Result<bool>>;

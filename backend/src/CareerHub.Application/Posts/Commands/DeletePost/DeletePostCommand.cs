using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Posts.Commands.DeletePost;

public record DeletePostCommand(Guid PostId, Guid UsuarioId) : IRequest<Result<bool>>;

using CareerHub.Application.Posts.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Posts.Commands.UpdatePost;

public record UpdatePostCommand(Guid PostId, UpdatePostDto Dto, Guid UsuarioId) : IRequest<Result<PostDto>>;

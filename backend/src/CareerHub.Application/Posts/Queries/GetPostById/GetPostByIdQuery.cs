using CareerHub.Application.Posts.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Posts.Queries.GetPostById;

public record GetPostByIdQuery(Guid PostId, Guid UsuarioId) : IRequest<Result<PostDto>>;

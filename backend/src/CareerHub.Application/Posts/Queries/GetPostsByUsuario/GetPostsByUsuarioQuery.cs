using CareerHub.Application.Posts.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Posts.Queries.GetPostsByUsuario;

public record GetPostsByUsuarioQuery(Guid UsuarioId) : IRequest<Result<IEnumerable<PostDto>>>;

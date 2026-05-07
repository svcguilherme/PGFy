using AutoMapper;
using CareerHub.Application.Posts.DTOs;
using CareerHub.Domain.Posts.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Posts.Queries.GetPostsByUsuario;

public class GetPostsByUsuarioHandler(IPostRepository repo, IMapper mapper)
    : IRequestHandler<GetPostsByUsuarioQuery, Result<IEnumerable<PostDto>>>
{
    public async Task<Result<IEnumerable<PostDto>>> Handle(GetPostsByUsuarioQuery query, CancellationToken ct)
    {
        var posts = await repo.GetByUsuarioAsync(query.UsuarioId, ct);
        return Result.Success(mapper.Map<IEnumerable<PostDto>>(posts));
    }
}

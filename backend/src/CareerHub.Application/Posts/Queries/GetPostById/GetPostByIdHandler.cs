using AutoMapper;
using CareerHub.Application.Posts.DTOs;
using CareerHub.Domain.Posts.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Posts.Queries.GetPostById;

public class GetPostByIdHandler(IPostRepository repo, IMapper mapper)
    : IRequestHandler<GetPostByIdQuery, Result<PostDto>>
{
    public async Task<Result<PostDto>> Handle(GetPostByIdQuery query, CancellationToken ct)
    {
        var post = await repo.GetByIdAsync(query.PostId, ct);
        if (post is null)
            return Result.Failure<PostDto>("Post não encontrado.");

        if (post.UsuarioId != query.UsuarioId)
            return Result.Failure<PostDto>("Acesso negado.");

        return Result.Success(mapper.Map<PostDto>(post));
    }
}

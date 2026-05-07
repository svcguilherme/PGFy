using AutoMapper;
using CareerHub.Application.Posts.DTOs;
using CareerHub.Domain.Posts.Entities;
using CareerHub.Domain.Posts.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Posts.Commands.CreatePost;

public class CreatePostHandler(IPostRepository repo, IMapper mapper)
    : IRequestHandler<CreatePostCommand, Result<PostDto>>
{
    public async Task<Result<PostDto>> Handle(CreatePostCommand cmd, CancellationToken ct)
    {
        var result = Post.Create(cmd.Dto.Titulo, cmd.Dto.Conteudo, cmd.UsuarioId);
        if (!result.IsSuccess)
            return Result.Failure<PostDto>(result.Error!);

        await repo.AddAsync(result.Value!, ct);
        return Result.Success(mapper.Map<PostDto>(result.Value!));
    }
}

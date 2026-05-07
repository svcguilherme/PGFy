using AutoMapper;
using CareerHub.Application.Posts.DTOs;
using CareerHub.Domain.Posts.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Posts.Commands.UpdatePost;

public class UpdatePostHandler(IPostRepository repo, IMapper mapper)
    : IRequestHandler<UpdatePostCommand, Result<PostDto>>
{
    public async Task<Result<PostDto>> Handle(UpdatePostCommand cmd, CancellationToken ct)
    {
        var post = await repo.GetByIdAsync(cmd.PostId, ct);
        if (post is null)
            return Result.Failure<PostDto>("Post não encontrado.");

        if (post.UsuarioId != cmd.UsuarioId)
            return Result.Failure<PostDto>("Acesso negado.");

        var update = post.Atualizar(cmd.Dto.Titulo, cmd.Dto.Conteudo);
        if (!update.IsSuccess)
            return Result.Failure<PostDto>(update.Error!);

        await repo.UpdateAsync(post, ct);
        return Result.Success(mapper.Map<PostDto>(post));
    }
}

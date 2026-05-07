using CareerHub.Domain.Posts.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Posts.Commands.DeletePost;

public class DeletePostHandler(IPostRepository repo)
    : IRequestHandler<DeletePostCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeletePostCommand cmd, CancellationToken ct)
    {
        var post = await repo.GetByIdAsync(cmd.PostId, ct);
        if (post is null)
            return Result.Failure<bool>("Post não encontrado.");

        if (post.UsuarioId != cmd.UsuarioId)
            return Result.Failure<bool>("Acesso negado.");

        await repo.DeleteAsync(cmd.PostId, ct);
        return Result.Success(true);
    }
}

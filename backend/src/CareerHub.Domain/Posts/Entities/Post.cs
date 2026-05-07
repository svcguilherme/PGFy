using CareerHub.SharedKernel;

namespace CareerHub.Domain.Posts.Entities;

public class Post : AuditableEntity
{
    public string Titulo { get; private set; } = string.Empty;
    public string Conteudo { get; private set; } = string.Empty;
    public DateTime DataPublicacao { get; private set; }
    public Guid UsuarioId { get; private set; }

    private Post() { }

    public static Result<Post> Create(string titulo, string conteudo, Guid usuarioId)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Failure<Post>("Título é obrigatório.");
        if (string.IsNullOrWhiteSpace(conteudo))
            return Result.Failure<Post>("Conteúdo é obrigatório.");

        var post = new Post
        {
            Titulo = titulo.Trim(),
            Conteudo = conteudo,
            DataPublicacao = DateTime.UtcNow,
            UsuarioId = usuarioId
        };
        post.SetCreatedBy(usuarioId);
        post.AddDomainEvent(new PostCriadoEvent(post.Id));
        return Result.Success(post);
    }

    public Result<bool> Atualizar(string titulo, string conteudo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Failure<bool>("Título é obrigatório.");
        if (string.IsNullOrWhiteSpace(conteudo))
            return Result.Failure<bool>("Conteúdo é obrigatório.");

        Titulo = titulo.Trim();
        Conteudo = conteudo;
        SetUpdated();
        return Result.Success(true);
    }
}

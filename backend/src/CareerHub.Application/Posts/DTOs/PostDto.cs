namespace CareerHub.Application.Posts.DTOs;

public record PostDto(Guid Id, string Titulo, string Conteudo, DateTime DataPublicacao, Guid UsuarioId);

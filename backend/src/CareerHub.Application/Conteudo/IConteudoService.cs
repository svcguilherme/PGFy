using CareerHub.Application.Conteudo.DTOs;

namespace CareerHub.Application.Conteudo;

public interface IConteudoService
{
    Task<IEnumerable<ConteudoItemDto>> GetConteudoAsync(CancellationToken ct = default);
}

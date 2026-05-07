using CareerHub.Application.Conteudo.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Conteudo.Queries.GetConteudo;

public class GetConteudoHandler(IConteudoService conteudoService)
    : IRequestHandler<GetConteudoQuery, Result<IEnumerable<ConteudoItemDto>>>
{
    public async Task<Result<IEnumerable<ConteudoItemDto>>> Handle(GetConteudoQuery query, CancellationToken ct)
    {
        var itens = await conteudoService.GetConteudoAsync(ct);
        return Result.Success(itens);
    }
}

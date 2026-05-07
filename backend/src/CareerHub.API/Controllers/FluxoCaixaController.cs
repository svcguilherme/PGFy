using CareerHub.Application.Financeiro.Queries.GetFluxoMensal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub.API.Controllers;

[Authorize]
[Route("api/v1/fluxo-caixa")]
public class FluxoCaixaController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("{ano:int}/{mes:int}")]
    public async Task<IActionResult> GetFluxoMensal(int ano, int mes)
    {
        var result = await mediator.Send(new GetFluxoMensalQuery(UserId, ano, mes));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}

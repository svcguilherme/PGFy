using CareerHub.Application.Conteudo.Queries.GetConteudo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub.API.Controllers;

[Route("api/v1/conteudo")]
public class ConteudoController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetConteudo()
    {
        var result = await mediator.Send(new GetConteudoQuery());
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}

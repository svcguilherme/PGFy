using CareerHub.Application.Financeiro.Commands.CreateRecebivel;
using CareerHub.Application.Financeiro.Commands.DeleteRecebivel;
using CareerHub.Application.Financeiro.Commands.MarcarRecebivelComoRecebido;
using CareerHub.Application.Financeiro.Commands.UpdateRecebivel;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Application.Financeiro.Queries.GetReceiveisByUsuario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub.API.Controllers;

[Authorize]
[Route("api/v1/recebiveis")]
public class ReceiveisController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetReceiveisByUsuarioQuery(UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRecebivelDto dto)
    {
        var result = await mediator.Send(new CreateRecebivelCommand(dto, UserId));
        return result.IsSuccess ? CreatedAtAction(nameof(GetAll), result.Value) : BadRequest(result.Error);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateRecebivelDto dto)
    {
        var result = await mediator.Send(new UpdateRecebivelCommand(id, dto, UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteRecebivelCommand(id, UserId));
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpPatch("{id:guid}/recebido")]
    public async Task<IActionResult> MarcarComoRecebido(Guid id)
    {
        var result = await mediator.Send(new MarcarRecebivelComoRecebidoCommand(id, UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}

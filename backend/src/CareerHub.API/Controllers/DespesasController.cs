using CareerHub.Application.Financeiro.Commands.CreateDespesa;
using CareerHub.Application.Financeiro.Commands.DeleteDespesa;
using CareerHub.Application.Financeiro.Commands.MarcarDespesaComoPago;
using CareerHub.Application.Financeiro.Commands.UpdateDespesa;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Application.Financeiro.Queries.GetDespesasByUsuario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub.API.Controllers;

[Authorize]
[Route("api/v1/despesas")]
public class DespesasController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetDespesasByUsuarioQuery(UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDespesaDto dto)
    {
        var result = await mediator.Send(new CreateDespesaCommand(dto, UserId));
        return result.IsSuccess ? CreatedAtAction(nameof(GetAll), result.Value) : BadRequest(result.Error);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateDespesaDto dto)
    {
        var result = await mediator.Send(new UpdateDespesaCommand(id, dto, UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteDespesaCommand(id, UserId));
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpPatch("{id:guid}/pago")]
    public async Task<IActionResult> MarcarComoPago(Guid id)
    {
        var result = await mediator.Send(new MarcarDespesaComoPagoCommand(id, UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}

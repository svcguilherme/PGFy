using CareerHub.Application.Estudos.Commands.CreateEstudo;
using CareerHub.Application.Estudos.Commands.DeleteEstudo;
using CareerHub.Application.Estudos.Commands.UpdateEstudo;
using CareerHub.Application.Estudos.DTOs;
using CareerHub.Application.Estudos.Queries.GetEstudosByDia;
using CareerHub.Application.Estudos.Queries.GetEstudosByUsuario;
using CareerHub.Domain.Estudos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub.API.Controllers;

[Authorize]
[Route("api/v1/estudos")]
public class EstudosController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetEstudosByUsuarioQuery(UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("por-dia/{dia}")]
    public async Task<IActionResult> GetByDia(DiaDaSemana dia)
    {
        var result = await mediator.Send(new GetEstudosByDiaQuery(UserId, dia));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEstudoDto dto)
    {
        var result = await mediator.Send(new CreateEstudoCommand(dto, UserId));
        return result.IsSuccess ? CreatedAtAction(nameof(GetAll), result.Value) : BadRequest(result.Error);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateEstudoDto dto)
    {
        var result = await mediator.Send(new UpdateEstudoCommand(id, dto, UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteEstudoCommand(id, UserId));
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}

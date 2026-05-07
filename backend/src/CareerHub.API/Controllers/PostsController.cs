using CareerHub.Application.Posts.Commands.CreatePost;
using CareerHub.Application.Posts.Commands.DeletePost;
using CareerHub.Application.Posts.Commands.UpdatePost;
using CareerHub.Application.Posts.DTOs;
using CareerHub.Application.Posts.Queries.GetPostById;
using CareerHub.Application.Posts.Queries.GetPostsByUsuario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerHub.API.Controllers;

[Authorize]
[Route("api/v1/posts")]
public class PostsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetPostsByUsuarioQuery(UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetPostByIdQuery(id, UserId));
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePostDto dto)
    {
        var result = await mediator.Send(new CreatePostCommand(dto, UserId));
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value) : BadRequest(result.Error);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdatePostDto dto)
    {
        var result = await mediator.Send(new UpdatePostCommand(id, dto, UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeletePostCommand(id, UserId));
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}

using CareerHub.Application.Posts.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Posts.Commands.CreatePost;

public record CreatePostCommand(CreatePostDto Dto, Guid UsuarioId) : IRequest<Result<PostDto>>;

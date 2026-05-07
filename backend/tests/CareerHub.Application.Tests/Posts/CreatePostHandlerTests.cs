using AutoMapper;
using CareerHub.Application.Posts.Commands.CreatePost;
using CareerHub.Application.Posts.DTOs;
using CareerHub.Domain.Posts.Entities;
using CareerHub.Domain.Posts.Interfaces;
using FluentAssertions;
using Moq;

namespace CareerHub.Application.Tests.Posts;

public class CreatePostHandlerTests
{
    private readonly Mock<IPostRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly CreatePostHandler _handler;

    public CreatePostHandlerTests()
    {
        _handler = new CreatePostHandler(_repo.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_ValidInput_ReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var dto = new CreatePostDto("Meu primeiro post", "Conteúdo do post sobre .NET");
        var expectedDto = new PostDto(Guid.NewGuid(), dto.Titulo, dto.Conteudo, DateTime.UtcNow, userId);

        _repo.Setup(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _mapper.Setup(m => m.Map<PostDto>(It.IsAny<Post>())).Returns(expectedDto);

        var result = await _handler.Handle(new CreatePostCommand(dto, userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        _repo.Verify(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_TituloVazio_ReturnsFailure()
    {
        var dto = new CreatePostDto("", "Conteúdo válido do post");

        var result = await _handler.Handle(new CreatePostCommand(dto, Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
        _repo.Verify(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ConteudoVazio_ReturnsFailure()
    {
        var dto = new CreatePostDto("Titulo válido", "");

        var result = await _handler.Handle(new CreatePostCommand(dto, Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
        _repo.Verify(r => r.AddAsync(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

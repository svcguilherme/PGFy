using CareerHub.Application.Estudos.Commands.DeleteEstudo;
using CareerHub.Domain.Estudos;
using CareerHub.Domain.Estudos.Entities;
using CareerHub.Domain.Estudos.Interfaces;
using FluentAssertions;
using Moq;

namespace CareerHub.Application.Tests.Estudos;

public class DeleteEstudoHandlerTests
{
    private readonly Mock<IEstudoRepository> _repo = new();
    private readonly DeleteEstudoHandler _handler;

    public DeleteEstudoHandlerTests()
    {
        _handler = new DeleteEstudoHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_EstudoExistente_ReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var estudo = Estudo.Create("Algoritmos", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), userId).Value!;

        _repo.Setup(r => r.GetByIdAsync(estudo.Id, It.IsAny<CancellationToken>())).ReturnsAsync(estudo);
        _repo.Setup(r => r.DeleteAsync(estudo.Id, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _handler.Handle(new DeleteEstudoCommand(estudo.Id, userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
        _repo.Verify(r => r.DeleteAsync(estudo.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_EstudoNaoEncontrado_ReturnsFailure()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Estudo?)null);

        var result = await _handler.Handle(new DeleteEstudoCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("não encontrado");
    }

    [Fact]
    public async Task Handle_UsuarioDiferente_ReturnsFailure()
    {
        var ownerId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var estudo = Estudo.Create("Algoritmos", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), ownerId).Value!;

        _repo.Setup(r => r.GetByIdAsync(estudo.Id, It.IsAny<CancellationToken>())).ReturnsAsync(estudo);

        var result = await _handler.Handle(new DeleteEstudoCommand(estudo.Id, differentUserId), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Acesso negado");
        _repo.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

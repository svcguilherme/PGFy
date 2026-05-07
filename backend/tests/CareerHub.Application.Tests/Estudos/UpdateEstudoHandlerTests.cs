using AutoMapper;
using CareerHub.Application.Estudos.Commands.UpdateEstudo;
using CareerHub.Application.Estudos.DTOs;
using CareerHub.Domain.Estudos;
using CareerHub.Domain.Estudos.Entities;
using CareerHub.Domain.Estudos.Interfaces;
using FluentAssertions;
using Moq;

namespace CareerHub.Application.Tests.Estudos;

public class UpdateEstudoHandlerTests
{
    private readonly Mock<IEstudoRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly UpdateEstudoHandler _handler;

    public UpdateEstudoHandlerTests()
    {
        _handler = new UpdateEstudoHandler(_repo.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_ValidInput_ReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var estudo = Estudo.Create("Algoritmos", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), userId).Value!;
        var updateDto = new UpdateEstudoDto("Algoritmos Avançados", DiaDaSemana.Terca, new TimeOnly(9, 0), new TimeOnly(11, 0), null);
        var expectedDto = new EstudoDto(estudo.Id, "Algoritmos Avançados", DiaDaSemana.Terca, new TimeOnly(9, 0), new TimeOnly(11, 0), 2.0, null, userId);

        _repo.Setup(r => r.GetByIdAsync(estudo.Id, It.IsAny<CancellationToken>())).ReturnsAsync(estudo);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Estudo>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _mapper.Setup(m => m.Map<EstudoDto>(It.IsAny<Estudo>())).Returns(expectedDto);

        var result = await _handler.Handle(new UpdateEstudoCommand(estudo.Id, updateDto, userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _repo.Verify(r => r.UpdateAsync(It.IsAny<Estudo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_EstudoNaoEncontrado_ReturnsFailure()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Estudo?)null);
        var updateDto = new UpdateEstudoDto("Titulo", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), null);

        var result = await _handler.Handle(new UpdateEstudoCommand(Guid.NewGuid(), updateDto, Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("não encontrado");
    }

    [Fact]
    public async Task Handle_UsuarioDiferente_ReturnsFailure()
    {
        var ownerId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var estudo = Estudo.Create("Algoritmos", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), ownerId).Value!;
        var updateDto = new UpdateEstudoDto("Titulo", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), null);

        _repo.Setup(r => r.GetByIdAsync(estudo.Id, It.IsAny<CancellationToken>())).ReturnsAsync(estudo);

        var result = await _handler.Handle(new UpdateEstudoCommand(estudo.Id, updateDto, differentUserId), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Acesso negado");
        _repo.Verify(r => r.UpdateAsync(It.IsAny<Estudo>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_TituloVazio_ReturnsFailure()
    {
        var userId = Guid.NewGuid();
        var estudo = Estudo.Create("Algoritmos", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), userId).Value!;
        var updateDto = new UpdateEstudoDto("", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), null);

        _repo.Setup(r => r.GetByIdAsync(estudo.Id, It.IsAny<CancellationToken>())).ReturnsAsync(estudo);

        var result = await _handler.Handle(new UpdateEstudoCommand(estudo.Id, updateDto, userId), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _repo.Verify(r => r.UpdateAsync(It.IsAny<Estudo>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

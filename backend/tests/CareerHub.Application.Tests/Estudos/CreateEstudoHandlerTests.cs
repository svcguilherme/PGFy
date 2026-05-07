using AutoMapper;
using CareerHub.Application.Estudos.Commands.CreateEstudo;
using CareerHub.Application.Estudos.DTOs;
using CareerHub.Domain.Estudos;
using CareerHub.Domain.Estudos.Interfaces;
using FluentAssertions;
using Moq;

namespace CareerHub.Application.Tests.Estudos;

public class CreateEstudoHandlerTests
{
    private readonly Mock<IEstudoRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly CreateEstudoHandler _handler;

    public CreateEstudoHandlerTests()
    {
        _handler = new CreateEstudoHandler(_repo.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_ValidInput_ReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var dto = new CreateEstudoDto("Algoritmos", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), null);
        var cmd = new CreateEstudoCommand(dto, userId);
        var expectedDto = new EstudoDto(Guid.NewGuid(), "Algoritmos", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), 2.0, null, userId);

        _repo.Setup(r => r.AddAsync(It.IsAny<Domain.Estudos.Entities.Estudo>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mapper.Setup(m => m.Map<EstudoDto>(It.IsAny<Domain.Estudos.Entities.Estudo>()))
            .Returns(expectedDto);

        var result = await _handler.Handle(cmd, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        _repo.Verify(r => r.AddAsync(It.IsAny<Domain.Estudos.Entities.Estudo>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_TituloVazio_ReturnsFailure()
    {
        var dto = new CreateEstudoDto("", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), null);
        var cmd = new CreateEstudoCommand(dto, Guid.NewGuid());

        var result = await _handler.Handle(cmd, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
        _repo.Verify(r => r.AddAsync(It.IsAny<Domain.Estudos.Entities.Estudo>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_HoraFimAnteriorInicio_ReturnsFailure()
    {
        var dto = new CreateEstudoDto("Algoritmos", DiaDaSemana.Segunda, new TimeOnly(10, 0), new TimeOnly(8, 0), null);
        var cmd = new CreateEstudoCommand(dto, Guid.NewGuid());

        var result = await _handler.Handle(cmd, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
        _repo.Verify(r => r.AddAsync(It.IsAny<Domain.Estudos.Entities.Estudo>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

using AutoMapper;
using CareerHub.Application.Financeiro.Commands.CreateDespesa;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro;
using CareerHub.Domain.Financeiro.Entities;
using CareerHub.Domain.Financeiro.Interfaces;
using FluentAssertions;
using Moq;

namespace CareerHub.Application.Tests.Financeiro;

public class CreateDespesaHandlerTests
{
    private readonly Mock<IDespesaRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly CreateDespesaHandler _handler;

    public CreateDespesaHandlerTests()
    {
        _handler = new CreateDespesaHandler(_repo.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_ValidInput_ReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var dto = new CreateDespesaDto("Aluguel", 1500m, DateTime.UtcNow.AddDays(10), CategoriaDespesa.Outros);
        var expectedDto = new DespesaDto(Guid.NewGuid(), "Aluguel", 1500m, DateTime.UtcNow, false, CategoriaDespesa.Outros, userId);

        _repo.Setup(r => r.AddAsync(It.IsAny<Despesa>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _mapper.Setup(m => m.Map<DespesaDto>(It.IsAny<Despesa>())).Returns(expectedDto);

        var result = await _handler.Handle(new CreateDespesaCommand(dto, userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        _repo.Verify(r => r.AddAsync(It.IsAny<Despesa>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValorZero_ReturnsFailure()
    {
        var dto = new CreateDespesaDto("Aluguel", 0m, DateTime.UtcNow, CategoriaDespesa.Outros);

        var result = await _handler.Handle(new CreateDespesaCommand(dto, Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
        _repo.Verify(r => r.AddAsync(It.IsAny<Despesa>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValorNegativo_ReturnsFailure()
    {
        var dto = new CreateDespesaDto("Aluguel", -100m, DateTime.UtcNow, CategoriaDespesa.Outros);

        var result = await _handler.Handle(new CreateDespesaCommand(dto, Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        _repo.Verify(r => r.AddAsync(It.IsAny<Despesa>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DescricaoVazia_ReturnsFailure()
    {
        var dto = new CreateDespesaDto("", 500m, DateTime.UtcNow, CategoriaDespesa.Alimentacao);

        var result = await _handler.Handle(new CreateDespesaCommand(dto, Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNullOrEmpty();
        _repo.Verify(r => r.AddAsync(It.IsAny<Despesa>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

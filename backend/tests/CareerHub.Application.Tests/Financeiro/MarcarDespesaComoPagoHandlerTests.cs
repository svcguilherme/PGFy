using AutoMapper;
using CareerHub.Application.Financeiro.Commands.MarcarDespesaComoPago;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro;
using CareerHub.Domain.Financeiro.Entities;
using CareerHub.Domain.Financeiro.Interfaces;
using FluentAssertions;
using Moq;

namespace CareerHub.Application.Tests.Financeiro;

public class MarcarDespesaComoPagoHandlerTests
{
    private readonly Mock<IDespesaRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly MarcarDespesaComoPagoHandler _handler;

    public MarcarDespesaComoPagoHandlerTests()
    {
        _handler = new MarcarDespesaComoPagoHandler(_repo.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_DespesaNaoPaga_MarcaComoPagoERetorna()
    {
        var userId = Guid.NewGuid();
        var despesa = Despesa.Create("Aluguel", 1500m, DateTime.UtcNow, CategoriaDespesa.Outros, userId).Value!;

        _repo.Setup(r => r.GetByIdAsync(despesa.Id, It.IsAny<CancellationToken>())).ReturnsAsync(despesa);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Despesa>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _mapper.Setup(m => m.Map<DespesaDto>(It.IsAny<Despesa>()))
            .Returns<Despesa>(d => new DespesaDto(d.Id, d.Descricao, d.Valor, d.DataPrevista, d.Pago, d.Categoria, d.UsuarioId));

        var result = await _handler.Handle(new MarcarDespesaComoPagoCommand(despesa.Id, userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Pago.Should().BeTrue();
        _repo.Verify(r => r.UpdateAsync(It.IsAny<Despesa>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DespesaJaPaga_IdempotenteSemErro()
    {
        var userId = Guid.NewGuid();
        var despesa = Despesa.Create("Aluguel", 1500m, DateTime.UtcNow, CategoriaDespesa.Outros, userId).Value!;
        despesa.MarcarComoPago();

        _repo.Setup(r => r.GetByIdAsync(despesa.Id, It.IsAny<CancellationToken>())).ReturnsAsync(despesa);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Despesa>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _mapper.Setup(m => m.Map<DespesaDto>(It.IsAny<Despesa>()))
            .Returns<Despesa>(d => new DespesaDto(d.Id, d.Descricao, d.Valor, d.DataPrevista, d.Pago, d.Categoria, d.UsuarioId));

        var result = await _handler.Handle(new MarcarDespesaComoPagoCommand(despesa.Id, userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Pago.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_DespesaNaoEncontrada_ReturnsFailure()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync((Despesa?)null);

        var result = await _handler.Handle(new MarcarDespesaComoPagoCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("não encontrada");
    }

    [Fact]
    public async Task Handle_UsuarioDiferente_ReturnsFailure()
    {
        var ownerId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var despesa = Despesa.Create("Aluguel", 1500m, DateTime.UtcNow, CategoriaDespesa.Outros, ownerId).Value!;

        _repo.Setup(r => r.GetByIdAsync(despesa.Id, It.IsAny<CancellationToken>())).ReturnsAsync(despesa);

        var result = await _handler.Handle(new MarcarDespesaComoPagoCommand(despesa.Id, differentUserId), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Acesso negado");
        _repo.Verify(r => r.UpdateAsync(It.IsAny<Despesa>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

using CareerHub.Application.Financeiro.Queries.GetFluxoMensal;
using CareerHub.Domain.Financeiro;
using CareerHub.Domain.Financeiro.Entities;
using CareerHub.Domain.Financeiro.Interfaces;
using FluentAssertions;
using Moq;

namespace CareerHub.Application.Tests.Financeiro;

public class GetFluxoMensalHandlerTests
{
    private readonly Mock<IDespesaRepository> _despesaRepo = new();
    private readonly Mock<IRecebivelRepository> _recebivelRepo = new();
    private readonly GetFluxoMensalHandler _handler;

    public GetFluxoMensalHandlerTests()
    {
        _handler = new GetFluxoMensalHandler(_despesaRepo.Object, _recebivelRepo.Object);
    }

    [Fact]
    public async Task Handle_RecebivelMaiorQueDespesa_SaldoPositivo()
    {
        var userId = Guid.NewGuid();
        var mes = new DateTime(2025, 1, 15);

        var despesas = new List<Despesa>
        {
            Despesa.Create("Aluguel", 3000m, mes, CategoriaDespesa.Outros, userId).Value!
        };
        var recebiveis = new List<Recebivel>
        {
            Recebivel.Create("Salário", 5000m, mes, userId).Value!
        };

        _despesaRepo.Setup(r => r.GetByMesAsync(userId, 2025, 1, It.IsAny<CancellationToken>())).ReturnsAsync(despesas);
        _recebivelRepo.Setup(r => r.GetByMesAsync(userId, 2025, 1, It.IsAny<CancellationToken>())).ReturnsAsync(recebiveis);

        var result = await _handler.Handle(new GetFluxoMensalQuery(userId, 2025, 1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Saldo.Should().Be(2000m);
        result.Value!.Positivo.Should().BeTrue();
        result.Value!.TotalRecebiveis.Should().Be(5000m);
        result.Value!.TotalDespesas.Should().Be(3000m);
    }

    [Fact]
    public async Task Handle_DespesaMaiorQueRecebivel_SaldoNegativo()
    {
        var userId = Guid.NewGuid();
        var mes = new DateTime(2025, 1, 15);

        var despesas = new List<Despesa>
        {
            Despesa.Create("Aluguel", 4000m, mes, CategoriaDespesa.Outros, userId).Value!
        };
        var recebiveis = new List<Recebivel>
        {
            Recebivel.Create("Freelance", 2000m, mes, userId).Value!
        };

        _despesaRepo.Setup(r => r.GetByMesAsync(userId, 2025, 1, It.IsAny<CancellationToken>())).ReturnsAsync(despesas);
        _recebivelRepo.Setup(r => r.GetByMesAsync(userId, 2025, 1, It.IsAny<CancellationToken>())).ReturnsAsync(recebiveis);

        var result = await _handler.Handle(new GetFluxoMensalQuery(userId, 2025, 1), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Saldo.Should().Be(-2000m);
        result.Value!.Positivo.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_MesSemMovimento_SaldoZero()
    {
        var userId = Guid.NewGuid();

        _despesaRepo.Setup(r => r.GetByMesAsync(userId, 2025, 6, It.IsAny<CancellationToken>())).ReturnsAsync([]);
        _recebivelRepo.Setup(r => r.GetByMesAsync(userId, 2025, 6, It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var result = await _handler.Handle(new GetFluxoMensalQuery(userId, 2025, 6), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Saldo.Should().Be(0m);
        result.Value!.Positivo.Should().BeTrue();
    }
}

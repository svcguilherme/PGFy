---
name: finance-patterns
description: Use this skill when working with Despesas, Recebiveis, FluxoCaixa, financial projections, monthly cash flow calculations, or any financial module feature. Trigger on: "despesa", "recebivel", "fluxo de caixa", "financeiro", "projeção", "saldo".
---

# Finance Patterns — CareerHub

## Regras de Negócio Financeiras

### Fluxo de Caixa Mensal
- Calculado dinamicamente — NUNCA persistir o fluxo, apenas Despesas e Recebiveis
- Agrupamento por `Ano + Mês` da `DataPrevista`
- `Saldo = TotalRecebiveis - TotalDespesas`
- Exibir meses com dados + próximos 3 meses projetados (mesmo sem lançamentos)

### Query de Fluxo de Caixa

```csharp
// Application/Financeiro/Queries/GetFluxoCaixaQuery.cs
public record GetFluxoCaixaQuery(Guid UsuarioId, int AnoInicio, int MesInicio,
    int AnoFim, int MesFim) : IRequest<Result<IEnumerable<FluxoMensalDto>>>;

// Handler
public class GetFluxoCaixaHandler(
    IDespesaRepository despesas,
    IRecebivelRepository recebiveis) : IRequestHandler<...>
{
    public async Task<Result<IEnumerable<FluxoMensalDto>>> Handle(...)
    {
        var despesasPeriodo = await despesas.GetByPeriodoAsync(
            usuarioId, inicio, fim, ct);
        var recebiveisPeriodo = await recebiveis.GetByPeriodoAsync(
            usuarioId, inicio, fim, ct);

        // Gera todos os meses no range, mesmo sem lançamentos
        var meses = GerarMesesNoPeriodo(inicio, fim);

        return meses.Select(m => new FluxoMensalDto
        {
            Ano = m.Year, Mes = m.Month,
            NomeMes = m.ToString("MMMM/yyyy", new CultureInfo("pt-BR")),
            TotalDespesas = despesasPeriodo
                .Where(d => d.DataPrevista.Year == m.Year && d.DataPrevista.Month == m.Month)
                .Sum(d => d.Valor),
            TotalRecebiveis = recebiveisPeriodo
                .Where(r => r.DataPrevista.Year == m.Year && r.DataPrevista.Month == m.Month)
                .Sum(r => r.ValorPrevisto),
        }).Select(f => f with { Saldo = f.TotalRecebiveis - f.TotalDespesas });
    }
}
```

## DTOs Financeiros

```csharp
public record FluxoMensalDto
{
    public int Ano { get; init; }
    public int Mes { get; init; }
    public string NomeMes { get; init; } = string.Empty;
    public decimal TotalRecebiveis { get; init; }
    public decimal TotalDespesas { get; init; }
    public decimal Saldo { get; init; }
    public bool SaldoPositivo => Saldo >= 0;
    public IEnumerable<DespesaResumoDto> Despesas { get; init; } = [];
    public IEnumerable<RecebivelResumoDto> Recebiveis { get; init; } = [];
}

public record DespesaResumoDto(Guid Id, string Descricao, decimal Valor,
    DateTime DataPrevista, bool Pago, string Categoria);

public record RecebivelResumoDto(Guid Id, string Descricao, decimal ValorPrevisto,
    DateTime DataPrevista, bool Recebido);
```

## Repository Interfaces

```csharp
public interface IDespesaRepository
{
    Task<IEnumerable<Despesa>> GetByPeriodoAsync(Guid usuarioId,
        DateOnly inicio, DateOnly fim, CancellationToken ct = default);
    Task<IEnumerable<Despesa>> GetByMesAsync(Guid usuarioId, int ano, int mes, ct);
    // CRUD padrão...
}
```

## Endpoints Financeiros

```
GET  /api/v1/financeiro/fluxo?anoInicio=2025&mesInicio=1&anoFim=2025&mesFim=12
GET  /api/v1/despesas?mes=6&ano=2025
POST /api/v1/despesas
PUT  /api/v1/despesas/{id}
DELETE /api/v1/despesas/{id}
PATCH /api/v1/despesas/{id}/pagar

GET  /api/v1/recebiveis?mes=6&ano=2025
POST /api/v1/recebiveis
PUT  /api/v1/recebiveis/{id}
DELETE /api/v1/recebiveis/{id}
PATCH /api/v1/recebiveis/{id}/receber
```

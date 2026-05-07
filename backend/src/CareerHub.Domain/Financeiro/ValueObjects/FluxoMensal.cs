namespace CareerHub.Domain.Financeiro.ValueObjects;

public record FluxoMensal(int Ano, int Mes, decimal TotalRecebiveis, decimal TotalDespesas)
{
    public decimal Saldo => TotalRecebiveis - TotalDespesas;
    public bool Positivo => Saldo >= 0;
}

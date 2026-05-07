namespace CareerHub.Application.Financeiro.DTOs;

public record FluxoMensalDto(int Ano, int Mes, decimal TotalRecebiveis, decimal TotalDespesas, decimal Saldo, bool Positivo);

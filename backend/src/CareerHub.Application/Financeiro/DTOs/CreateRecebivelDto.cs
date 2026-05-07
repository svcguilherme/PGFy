namespace CareerHub.Application.Financeiro.DTOs;

public record CreateRecebivelDto(string Descricao, decimal ValorPrevisto, DateTime DataPrevista);

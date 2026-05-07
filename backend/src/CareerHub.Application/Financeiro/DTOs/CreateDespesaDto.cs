using CareerHub.Domain.Financeiro;

namespace CareerHub.Application.Financeiro.DTOs;

public record CreateDespesaDto(string Descricao, decimal Valor, DateTime DataPrevista, CategoriaDespesa Categoria);

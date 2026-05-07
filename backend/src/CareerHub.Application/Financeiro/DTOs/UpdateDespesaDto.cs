using CareerHub.Domain.Financeiro;

namespace CareerHub.Application.Financeiro.DTOs;

public record UpdateDespesaDto(string Descricao, decimal Valor, DateTime DataPrevista, CategoriaDespesa Categoria);

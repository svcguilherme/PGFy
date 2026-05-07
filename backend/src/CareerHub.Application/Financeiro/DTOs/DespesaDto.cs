using CareerHub.Domain.Financeiro;

namespace CareerHub.Application.Financeiro.DTOs;

public record DespesaDto(Guid Id, string Descricao, decimal Valor, DateTime DataPrevista, bool Pago, CategoriaDespesa Categoria, Guid UsuarioId);

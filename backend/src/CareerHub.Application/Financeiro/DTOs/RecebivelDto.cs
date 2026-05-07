namespace CareerHub.Application.Financeiro.DTOs;

public record RecebivelDto(Guid Id, string Descricao, decimal ValorPrevisto, DateTime DataPrevista, bool Recebido, Guid UsuarioId);

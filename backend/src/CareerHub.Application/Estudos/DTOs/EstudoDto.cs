using CareerHub.Domain.Estudos;

namespace CareerHub.Application.Estudos.DTOs;

public record EstudoDto(
    Guid Id,
    string Titulo,
    DiaDaSemana DiaDaSemana,
    TimeOnly HoraInicio,
    TimeOnly HoraFim,
    double HorasTotais,
    string? Descricao,
    Guid UsuarioId);

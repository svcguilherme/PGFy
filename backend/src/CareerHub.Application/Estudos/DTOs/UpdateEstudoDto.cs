using CareerHub.Domain.Estudos;

namespace CareerHub.Application.Estudos.DTOs;

public record UpdateEstudoDto(
    string Titulo,
    DiaDaSemana DiaDaSemana,
    TimeOnly HoraInicio,
    TimeOnly HoraFim,
    string? Descricao);

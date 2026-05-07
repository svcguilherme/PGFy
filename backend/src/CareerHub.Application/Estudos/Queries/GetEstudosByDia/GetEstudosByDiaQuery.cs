using CareerHub.Application.Estudos.DTOs;
using CareerHub.Domain.Estudos;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Estudos.Queries.GetEstudosByDia;

public record GetEstudosByDiaQuery(Guid UsuarioId, DiaDaSemana DiaDaSemana) : IRequest<Result<IEnumerable<EstudoDto>>>;

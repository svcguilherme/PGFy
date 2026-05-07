using CareerHub.Application.Estudos.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Estudos.Queries.GetEstudosByUsuario;

public record GetEstudosByUsuarioQuery(Guid UsuarioId) : IRequest<Result<IEnumerable<EstudoDto>>>;

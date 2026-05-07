using CareerHub.Application.Estudos.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Estudos.Commands.UpdateEstudo;

public record UpdateEstudoCommand(Guid EstudoId, UpdateEstudoDto Dto, Guid UsuarioId) : IRequest<Result<EstudoDto>>;

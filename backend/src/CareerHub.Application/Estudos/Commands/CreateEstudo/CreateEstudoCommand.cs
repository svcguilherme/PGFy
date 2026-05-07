using CareerHub.Application.Estudos.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Estudos.Commands.CreateEstudo;

public record CreateEstudoCommand(CreateEstudoDto Dto, Guid UsuarioId) : IRequest<Result<EstudoDto>>;

using CareerHub.Application.Financeiro.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.UpdateRecebivel;

public record UpdateRecebivelCommand(Guid RecebivelId, UpdateRecebivelDto Dto, Guid UsuarioId) : IRequest<Result<RecebivelDto>>;

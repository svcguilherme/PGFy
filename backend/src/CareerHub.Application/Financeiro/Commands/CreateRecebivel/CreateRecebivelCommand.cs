using CareerHub.Application.Financeiro.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.CreateRecebivel;

public record CreateRecebivelCommand(CreateRecebivelDto Dto, Guid UsuarioId) : IRequest<Result<RecebivelDto>>;

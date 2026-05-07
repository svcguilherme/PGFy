using CareerHub.Application.Financeiro.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.MarcarRecebivelComoRecebido;

public record MarcarRecebivelComoRecebidoCommand(Guid RecebivelId, Guid UsuarioId) : IRequest<Result<RecebivelDto>>;

using CareerHub.Application.Financeiro.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.MarcarDespesaComoPago;

public record MarcarDespesaComoPagoCommand(Guid DespesaId, Guid UsuarioId) : IRequest<Result<DespesaDto>>;

using CareerHub.Application.Financeiro.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.UpdateDespesa;

public record UpdateDespesaCommand(Guid DespesaId, UpdateDespesaDto Dto, Guid UsuarioId) : IRequest<Result<DespesaDto>>;

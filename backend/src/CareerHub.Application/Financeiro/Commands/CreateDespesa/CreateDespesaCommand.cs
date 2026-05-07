using CareerHub.Application.Financeiro.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.CreateDespesa;

public record CreateDespesaCommand(CreateDespesaDto Dto, Guid UsuarioId) : IRequest<Result<DespesaDto>>;

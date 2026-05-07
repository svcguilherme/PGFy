using CareerHub.Application.Financeiro.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Queries.GetDespesasByUsuario;

public record GetDespesasByUsuarioQuery(Guid UsuarioId) : IRequest<Result<IEnumerable<DespesaDto>>>;

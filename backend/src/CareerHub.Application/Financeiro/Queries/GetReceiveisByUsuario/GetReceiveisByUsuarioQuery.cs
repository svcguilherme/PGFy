using CareerHub.Application.Financeiro.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Queries.GetReceiveisByUsuario;

public record GetReceiveisByUsuarioQuery(Guid UsuarioId) : IRequest<Result<IEnumerable<RecebivelDto>>>;

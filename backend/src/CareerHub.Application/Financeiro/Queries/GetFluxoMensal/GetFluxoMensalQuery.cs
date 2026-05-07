using CareerHub.Application.Financeiro.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Queries.GetFluxoMensal;

public record GetFluxoMensalQuery(Guid UsuarioId, int Ano, int Mes) : IRequest<Result<FluxoMensalDto>>;

using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Queries.GetFluxoMensal;

public class GetFluxoMensalHandler(IDespesaRepository despesaRepo, IRecebivelRepository recebivelRepo)
    : IRequestHandler<GetFluxoMensalQuery, Result<FluxoMensalDto>>
{
    public async Task<Result<FluxoMensalDto>> Handle(GetFluxoMensalQuery query, CancellationToken ct)
    {
        var despesasTask = despesaRepo.GetByMesAsync(query.UsuarioId, query.Ano, query.Mes, ct);
        var receiveisTask = recebivelRepo.GetByMesAsync(query.UsuarioId, query.Ano, query.Mes, ct);
        await Task.WhenAll(despesasTask, receiveisTask);
        var despesas = despesasTask.Result;
        var recebiveis = receiveisTask.Result;

        decimal totalDespesas = despesas.Sum(d => d.Valor);
        decimal totalRecebiveis = recebiveis.Sum(r => r.ValorPrevisto);
        decimal saldo = totalRecebiveis - totalDespesas;

        return Result.Success(new FluxoMensalDto(query.Ano, query.Mes, totalRecebiveis, totalDespesas, saldo, saldo >= 0));
    }
}

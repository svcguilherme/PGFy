using AutoMapper;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.MarcarDespesaComoPago;

public class MarcarDespesaComoPagoHandler(IDespesaRepository repo, IMapper mapper)
    : IRequestHandler<MarcarDespesaComoPagoCommand, Result<DespesaDto>>
{
    public async Task<Result<DespesaDto>> Handle(MarcarDespesaComoPagoCommand cmd, CancellationToken ct)
    {
        var despesa = await repo.GetByIdAsync(cmd.DespesaId, ct);
        if (despesa is null)
            return Result.Failure<DespesaDto>("Despesa não encontrada.");

        if (despesa.UsuarioId != cmd.UsuarioId)
            return Result.Failure<DespesaDto>("Acesso negado.");

        despesa.MarcarComoPago();
        await repo.UpdateAsync(despesa, ct);
        return Result.Success(mapper.Map<DespesaDto>(despesa));
    }
}

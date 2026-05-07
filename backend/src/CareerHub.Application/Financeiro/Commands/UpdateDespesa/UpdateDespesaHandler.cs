using AutoMapper;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.UpdateDespesa;

public class UpdateDespesaHandler(IDespesaRepository repo, IMapper mapper)
    : IRequestHandler<UpdateDespesaCommand, Result<DespesaDto>>
{
    public async Task<Result<DespesaDto>> Handle(UpdateDespesaCommand cmd, CancellationToken ct)
    {
        var despesa = await repo.GetByIdAsync(cmd.DespesaId, ct);
        if (despesa is null)
            return Result.Failure<DespesaDto>("Despesa não encontrada.");

        if (despesa.UsuarioId != cmd.UsuarioId)
            return Result.Failure<DespesaDto>("Acesso negado.");

        var update = despesa.Atualizar(cmd.Dto.Descricao, cmd.Dto.Valor, cmd.Dto.DataPrevista, cmd.Dto.Categoria);
        if (!update.IsSuccess)
            return Result.Failure<DespesaDto>(update.Error!);

        await repo.UpdateAsync(despesa, ct);
        return Result.Success(mapper.Map<DespesaDto>(despesa));
    }
}

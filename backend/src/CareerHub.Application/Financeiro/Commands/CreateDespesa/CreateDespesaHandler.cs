using AutoMapper;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro.Entities;
using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.CreateDespesa;

public class CreateDespesaHandler(IDespesaRepository repo, IMapper mapper)
    : IRequestHandler<CreateDespesaCommand, Result<DespesaDto>>
{
    public async Task<Result<DespesaDto>> Handle(CreateDespesaCommand cmd, CancellationToken ct)
    {
        var result = Despesa.Create(cmd.Dto.Descricao, cmd.Dto.Valor, cmd.Dto.DataPrevista, cmd.Dto.Categoria, cmd.UsuarioId);
        if (!result.IsSuccess)
            return Result.Failure<DespesaDto>(result.Error!);

        await repo.AddAsync(result.Value!, ct);
        return Result.Success(mapper.Map<DespesaDto>(result.Value!));
    }
}

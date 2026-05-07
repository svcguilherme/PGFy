using AutoMapper;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro.Entities;
using CareerHub.Domain.Financeiro.Interfaces;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Financeiro.Commands.CreateRecebivel;

public class CreateRecebivelHandler(IRecebivelRepository repo, IMapper mapper)
    : IRequestHandler<CreateRecebivelCommand, Result<RecebivelDto>>
{
    public async Task<Result<RecebivelDto>> Handle(CreateRecebivelCommand cmd, CancellationToken ct)
    {
        var result = Recebivel.Create(cmd.Dto.Descricao, cmd.Dto.ValorPrevisto, cmd.Dto.DataPrevista, cmd.UsuarioId);
        if (!result.IsSuccess)
            return Result.Failure<RecebivelDto>(result.Error!);

        await repo.AddAsync(result.Value!, ct);
        return Result.Success(mapper.Map<RecebivelDto>(result.Value!));
    }
}

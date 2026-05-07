using AutoMapper;
using CareerHub.Application.Financeiro.DTOs;
using CareerHub.Domain.Financeiro.Entities;

namespace CareerHub.Application.Financeiro.Mappings;

public class FinanceiroProfile : Profile
{
    public FinanceiroProfile()
    {
        CreateMap<Despesa, DespesaDto>();
        CreateMap<Recebivel, RecebivelDto>();
    }
}

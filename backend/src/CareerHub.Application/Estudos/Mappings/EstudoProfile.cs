using AutoMapper;
using CareerHub.Application.Estudos.DTOs;
using CareerHub.Domain.Estudos.Entities;

namespace CareerHub.Application.Estudos.Mappings;

public class EstudoProfile : Profile
{
    public EstudoProfile()
    {
        CreateMap<Estudo, EstudoDto>()
            .ForMember(d => d.HorasTotais, o => o.MapFrom(s => s.HorasTotais));
    }
}

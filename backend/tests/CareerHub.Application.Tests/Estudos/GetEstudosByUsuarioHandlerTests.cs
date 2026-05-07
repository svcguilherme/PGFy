using AutoMapper;
using CareerHub.Application.Estudos.DTOs;
using CareerHub.Application.Estudos.Queries.GetEstudosByUsuario;
using CareerHub.Domain.Estudos;
using CareerHub.Domain.Estudos.Entities;
using CareerHub.Domain.Estudos.Interfaces;
using FluentAssertions;
using Moq;

namespace CareerHub.Application.Tests.Estudos;

public class GetEstudosByUsuarioHandlerTests
{
    private readonly Mock<IEstudoRepository> _repo = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly GetEstudosByUsuarioHandler _handler;

    public GetEstudosByUsuarioHandlerTests()
    {
        _handler = new GetEstudosByUsuarioHandler(_repo.Object, _mapper.Object);
    }

    [Fact]
    public async Task Handle_ComEstudos_RetornaLista()
    {
        var userId = Guid.NewGuid();
        var estudos = new List<Estudo>
        {
            Estudo.Create("Algoritmos", DiaDaSemana.Segunda, new TimeOnly(8, 0), new TimeOnly(10, 0), userId).Value!,
            Estudo.Create("Estruturas de Dados", DiaDaSemana.Quarta, new TimeOnly(14, 0), new TimeOnly(16, 0), userId).Value!
        };
        var dtos = estudos.Select(e => new EstudoDto(e.Id, e.Titulo, e.DiaDaSemana, e.HoraInicio, e.HoraFim, e.HorasTotais, e.Descricao, e.UsuarioId)).ToList();

        _repo.Setup(r => r.GetByUsuarioAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync(estudos);
        _mapper.Setup(m => m.Map<IEnumerable<EstudoDto>>(It.IsAny<IEnumerable<Estudo>>())).Returns(dtos);

        var result = await _handler.Handle(new GetEstudosByUsuarioQuery(userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_SemEstudos_RetornaListaVazia()
    {
        var userId = Guid.NewGuid();

        _repo.Setup(r => r.GetByUsuarioAsync(userId, It.IsAny<CancellationToken>())).ReturnsAsync([]);
        _mapper.Setup(m => m.Map<IEnumerable<EstudoDto>>(It.IsAny<IEnumerable<Estudo>>())).Returns([]);

        var result = await _handler.Handle(new GetEstudosByUsuarioQuery(userId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}

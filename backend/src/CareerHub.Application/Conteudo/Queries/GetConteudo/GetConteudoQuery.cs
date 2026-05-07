using CareerHub.Application.Conteudo.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Conteudo.Queries.GetConteudo;

public record GetConteudoQuery : IRequest<Result<IEnumerable<ConteudoItemDto>>>;

using CareerHub.Application.Identity.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Identity.Commands.Login;

public record LoginCommand(LoginDto Dto) : IRequest<Result<AuthResponseDto>>;

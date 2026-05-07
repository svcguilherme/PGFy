using CareerHub.Application.Identity.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Identity.Commands.Register;

public record RegisterCommand(RegisterDto Dto) : IRequest<Result<AuthResponseDto>>;

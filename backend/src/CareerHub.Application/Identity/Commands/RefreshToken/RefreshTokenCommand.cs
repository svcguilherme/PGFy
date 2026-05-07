using CareerHub.Application.Identity.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Identity.Commands.RefreshToken;

public record RefreshTokenCommand(RefreshTokenDto Dto) : IRequest<Result<AuthResponseDto>>;

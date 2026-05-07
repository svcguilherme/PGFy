using CareerHub.Application.Identity.DTOs;
using CareerHub.SharedKernel;
using MediatR;

namespace CareerHub.Application.Identity.Commands.Register;

public class RegisterHandler(IAuthService authService) : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
{
    public Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken ct)
        => authService.RegistrarAsync(request.Dto, ct);
}

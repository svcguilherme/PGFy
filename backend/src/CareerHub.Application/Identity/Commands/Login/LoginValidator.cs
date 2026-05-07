using FluentValidation;

namespace CareerHub.Application.Identity.Commands.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Dto.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(x => x.Dto.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.");
    }
}

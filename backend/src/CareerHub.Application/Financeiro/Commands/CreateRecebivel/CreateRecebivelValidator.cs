using FluentValidation;

namespace CareerHub.Application.Financeiro.Commands.CreateRecebivel;

public class CreateRecebivelValidator : AbstractValidator<CreateRecebivelCommand>
{
    public CreateRecebivelValidator()
    {
        RuleFor(x => x.Dto.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MaximumLength(300).WithMessage("Descrição não pode exceder 300 caracteres.");

        RuleFor(x => x.Dto.ValorPrevisto)
            .GreaterThan(0).WithMessage("Valor previsto deve ser positivo.");
    }
}

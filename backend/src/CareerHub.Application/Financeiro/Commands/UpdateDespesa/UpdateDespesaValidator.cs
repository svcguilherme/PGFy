using FluentValidation;

namespace CareerHub.Application.Financeiro.Commands.UpdateDespesa;

public class UpdateDespesaValidator : AbstractValidator<UpdateDespesaCommand>
{
    public UpdateDespesaValidator()
    {
        RuleFor(x => x.Dto.Descricao)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MaximumLength(300).WithMessage("Descrição não pode exceder 300 caracteres.");

        RuleFor(x => x.Dto.Valor)
            .GreaterThan(0).WithMessage("Valor deve ser positivo.");

        RuleFor(x => x.Dto.Categoria)
            .IsInEnum().WithMessage("Categoria inválida.");
    }
}

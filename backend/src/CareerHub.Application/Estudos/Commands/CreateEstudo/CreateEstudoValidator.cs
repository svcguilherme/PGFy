using FluentValidation;

namespace CareerHub.Application.Estudos.Commands.CreateEstudo;

public class CreateEstudoValidator : AbstractValidator<CreateEstudoCommand>
{
    public CreateEstudoValidator()
    {
        RuleFor(x => x.Dto.Titulo)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MaximumLength(200).WithMessage("Título não pode exceder 200 caracteres.");

        RuleFor(x => x.Dto.DiaDaSemana)
            .IsInEnum().WithMessage("Dia da semana inválido.");

        RuleFor(x => x.Dto.HoraFim)
            .GreaterThan(x => x.Dto.HoraInicio)
            .WithMessage("Hora de fim deve ser após hora de início.");
    }
}

using FluentValidation;

namespace CareerHub.Application.Posts.Commands.UpdatePost;

public class UpdatePostValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostValidator()
    {
        RuleFor(x => x.Dto.Titulo)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MaximumLength(300).WithMessage("Título não pode exceder 300 caracteres.");

        RuleFor(x => x.Dto.Conteudo)
            .NotEmpty().WithMessage("Conteúdo é obrigatório.");
    }
}

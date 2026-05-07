using FluentValidation;

namespace CareerHub.Application.Posts.Commands.CreatePost;

public class CreatePostValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.Dto.Titulo)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MaximumLength(300).WithMessage("Título não pode exceder 300 caracteres.");

        RuleFor(x => x.Dto.Conteudo)
            .NotEmpty().WithMessage("Conteúdo é obrigatório.");
    }
}

using FluentValidation;

namespace ProductsCatalog.Application.Features.Books.Commands.CreateBook;

public sealed class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Author).NotEmpty().MinimumLength(3).MaximumLength(30);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Genre).IsInEnum();
    }
}

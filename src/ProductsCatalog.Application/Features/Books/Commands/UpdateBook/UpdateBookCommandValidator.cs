using FluentValidation;

namespace ProductsCatalog.Application.Features.Books.Commands.UpdateBook;

public sealed class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Author).NotEmpty().MinimumLength(3).MaximumLength(30);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Genre).IsInEnum();
    }
}

using FluentValidation;

namespace ProductsCatalog.Application.Features.Stocks.Commands.CreateStock;

public sealed class CreateStockCommandValidator : AbstractValidator<CreateStockCommand>
{
    public CreateStockCommandValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}

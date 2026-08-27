using FluentValidation;

namespace ProductsCatalog.Application.Features.Stocks.Commands.AddStock;

public sealed class AddStockCommandValidator : AbstractValidator<AddStockCommand>
{
    public AddStockCommandValidator()
    {
        RuleFor(x => x.BookId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}

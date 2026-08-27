using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Stocks.Commands.AddStock;

public sealed class AddStockCommandHandler(IStockRepository stockRepository) : IRequestHandler<AddStockCommand, AddStockResult>
{
    public async Task<AddStockResult> Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await stockRepository.GetByBookIdAsync(request.BookId, cancellationToken)
            ?? throw new NotFoundException(nameof(Stock), request.BookId);

        stock.Replenish(request.Quantity);
        stockRepository.Update(stock);

        return new AddStockResult($"Stock updated! Available items: {stock.Quantity}");
    }
}

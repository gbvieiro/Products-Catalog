using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Stocks.Commands.UpdateStock;

public sealed class UpdateStockCommandHandler(IStockRepository stockRepository) : IRequestHandler<UpdateStockCommand, Unit>
{
    public async Task<Unit> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await stockRepository.GetByBookIdAsync(request.BookId, cancellationToken)
            ?? throw new NotFoundException(nameof(Stock), request.BookId);

        stock.SetQuantity(request.Quantity);
        stockRepository.Update(stock);

        return Unit.Value;
    }
}

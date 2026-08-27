using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Stocks.Commands.DeleteStock;

public sealed class DeleteStockCommandHandler(IStockRepository stockRepository) : IRequestHandler<DeleteStockCommand, Unit>
{
    public async Task<Unit> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await stockRepository.GetByBookIdAsync(request.BookId, cancellationToken)
            ?? throw new NotFoundException(nameof(Stock), request.BookId);

        stockRepository.Remove(stock);

        return Unit.Value;
    }
}

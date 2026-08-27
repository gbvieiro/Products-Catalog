using MediatR;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Stocks.Commands.CreateStock;

public sealed class CreateStockCommandHandler(IStockRepository stockRepository) : IRequestHandler<CreateStockCommand, Guid>
{
    public async Task<Guid> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        var stock = new Stock(request.BookId, request.Quantity);
        await stockRepository.AddAsync(stock, cancellationToken);
        return stock.Id;
    }
}

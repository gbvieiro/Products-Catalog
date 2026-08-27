using AutoMapper;
using MediatR;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Stocks.Queries.GetStockById;

public sealed class GetStockByIdQueryHandler(IStockRepository stockRepository, IMapper mapper)
    : IRequestHandler<GetStockByIdQuery, StockDto?>
{
    public async Task<StockDto?> Handle(GetStockByIdQuery request, CancellationToken cancellationToken)
    {
        var stock = await stockRepository.GetByIdAsync(request.Id, cancellationToken);
        return stock is null ? null : mapper.Map<StockDto>(stock);
    }
}

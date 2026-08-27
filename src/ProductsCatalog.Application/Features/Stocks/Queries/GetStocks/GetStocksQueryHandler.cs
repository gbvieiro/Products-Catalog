using AutoMapper;
using MediatR;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.Specifications;

namespace ProductsCatalog.Application.Features.Stocks.Queries.GetStocks;

public sealed class GetStocksQueryHandler(IStockRepository stockRepository, IMapper mapper)
    : IRequestHandler<GetStocksQuery, PagedResult<StockDto>>
{
    public async Task<PagedResult<StockDto>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
    {
        var spec = new StocksFilterSpecification(request.Filter, request.Skip, request.Take);

        var stocks = await stockRepository.ListAsync(spec, cancellationToken);
        var total = await stockRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<StockDto>(mapper.Map<List<StockDto>>(stocks), total, request.Skip, request.Take);
    }
}

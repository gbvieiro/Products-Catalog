using ProductsCatalog.Application.Common.Messaging;
using ProductsCatalog.Application.Common.Models;

namespace ProductsCatalog.Application.Features.Stocks.Queries.GetStocks;

public sealed record GetStocksQuery(string? Filter = null, int Skip = 0, int Take = 20) : IQuery<PagedResult<StockDto>>;

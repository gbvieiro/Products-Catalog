using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Stocks.Queries.GetStockById;

public sealed record GetStockByIdQuery(Guid Id) : IQuery<StockDto?>;

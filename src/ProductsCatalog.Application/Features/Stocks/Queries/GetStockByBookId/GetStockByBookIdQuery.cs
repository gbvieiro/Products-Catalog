using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Stocks.Queries.GetStockByBookId;

public sealed record GetStockByBookIdQuery(Guid BookId) : IQuery<CompleteStockDto?>;

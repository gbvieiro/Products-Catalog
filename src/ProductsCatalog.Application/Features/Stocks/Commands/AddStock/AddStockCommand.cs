using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Stocks.Commands.AddStock;

public sealed record AddStockCommand(Guid BookId, int Quantity) : ICommand<AddStockResult>;

public sealed record AddStockResult(string Message);

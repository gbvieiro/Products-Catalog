using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Stocks.Commands.CreateStock;

public sealed record CreateStockCommand(Guid BookId, int Quantity) : ICommand<Guid>;

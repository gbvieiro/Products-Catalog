using MediatR;
using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Stocks.Commands.DeleteStock;

public sealed record DeleteStockCommand(Guid BookId) : ICommand<Unit>;

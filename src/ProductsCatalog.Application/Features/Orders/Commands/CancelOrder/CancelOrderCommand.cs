using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Orders.Commands.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId) : ICommand<CancelOrderResult>;

public sealed record CancelOrderResult(string Message);

using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(Guid CustomerId, List<CreateOrderItemRequest> Items) : ICommand<Guid>;

namespace ProductsCatalog.Application.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderItemRequest(Guid BookId, int Quantity);

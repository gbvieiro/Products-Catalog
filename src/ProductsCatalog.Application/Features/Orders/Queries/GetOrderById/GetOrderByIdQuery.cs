using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid Id) : IQuery<OrderDto?>;

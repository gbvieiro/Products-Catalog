using ProductsCatalog.Application.Common.Mappings;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Application.Features.Orders;

public sealed record OrderItemDto : IMapFrom<OrderItem>
{
    public Guid BookId { get; init; }
    public int Quantity { get; init; }
    public double UnitPrice { get; init; }
    public double Amount { get; init; }
}

public sealed record OrderDto : IMapFrom<Order>
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public EOrderStatus Status { get; init; }
    public double TotalAmount { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyCollection<OrderItemDto> Items { get; init; } = [];
}

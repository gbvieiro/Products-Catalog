namespace ProductsCatalog.Domain.Events;

public sealed class OrderCreatedEvent(Guid orderId, Guid customerId) : BaseDomainEvent
{
    public Guid OrderId { get; } = orderId;
    public Guid CustomerId { get; } = customerId;
}

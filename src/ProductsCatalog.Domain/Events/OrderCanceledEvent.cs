namespace ProductsCatalog.Domain.Events;

public sealed class OrderCanceledEvent(Guid orderId) : BaseDomainEvent
{
    public Guid OrderId { get; } = orderId;
}

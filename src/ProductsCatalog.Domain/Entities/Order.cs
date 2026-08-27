using ProductsCatalog.Domain.Common;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.Events;
using ProductsCatalog.Domain.Exceptions;

namespace ProductsCatalog.Domain.Entities;

public class Order : BaseEntity
{
    private readonly List<OrderItem> _items = [];

    protected Order()
    {
    }

    public Order(Guid customerId, IEnumerable<OrderItem> items)
    {
        CustomerId = customerId;
        Status = EOrderStatus.Created;
        _items = items?.ToList() ?? [];

        Validate();

        TotalAmount = _items.Sum(item => item.Amount);

        AddDomainEvent(new OrderCreatedEvent(Id, CustomerId));
    }

    public Guid CustomerId { get; private set; }
    public EOrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public double TotalAmount { get; private set; }

    private void Validate()
    {
        DomainException.When(CustomerId == Guid.Empty, "Customer ID is required.");
        DomainException.When(_items.Count == 0, "An order must have order items.");
    }

    /// <summary>Confirma um pedido recem criado (ex: apos pagamento aprovado).</summary>
    public void Confirm()
    {
        DomainException.When(Status != EOrderStatus.Created, "Only created orders can be confirmed.");
        Status = EOrderStatus.Confirmed;
        Touch();
    }

    /// <summary>Cancela o pedido. A reposicao do estoque e responsabilidade do use case (Application).</summary>
    public void Cancel()
    {
        DomainException.When(Status == EOrderStatus.Canceled, "Order is already canceled.");
        Status = EOrderStatus.Canceled;
        Touch();

        AddDomainEvent(new OrderCanceledEvent(Id));
    }
}

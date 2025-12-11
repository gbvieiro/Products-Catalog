using Products.Catalog.Domain.Enums;
using Products.Catalog.Domain.Validations;
using Shared.Entities;

namespace Products.Catalog.Domain.Entities;

public class Order : Entity
{
    // Construtor sem parâmetros para o Entity Framework Core
    protected Order()
    {
        CustomerId = Guid.Empty;
        Status = EOrderStatus.Created;
        Items = new List<OrderItem>();
        TotalAmount = 0;
    }

    public Order(
        Guid customerId,
        EOrderStatus status,
        List<OrderItem> items,
        double totalAmount
    )
    {
        CustomerId = customerId;
        Status = status;
        Items = items;
        TotalAmount = totalAmount;

        ValidateBookDomain();
    }

    public Guid CustomerId { get; set; }
    public EOrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items { get; private set; }
    public double TotalAmount { get; private set; }

    private void ValidateBookDomain()
    {
        DomainException.When(Id == Guid.Empty, "ID is required");

        DomainException.When(CustomerId == Guid.Empty, "Custumer ID is required");

        DomainException.When(Items == null || !Items.Any(), "A order must have order items.");
    }

    public void SetStatus(EOrderStatus status)
    {
        Status = status;
    }

    public void UpdateTotalAmount()
    {
        TotalAmount = Items.Sum(x => x.Amount);
    }
}
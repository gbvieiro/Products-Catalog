using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities;

public sealed class OrderItem
{
    public Guid BookId { get; private set; }
    public int Quantity { get; private set; }
    public double Amount { get; private set; }

    // Construtor sem parâmetros para o Entity Framework Core
    public OrderItem()
    {
        BookId = Guid.Empty;
        Quantity = 0;
        Amount = 0;
    }

    public OrderItem(Guid bookId, int quantity, double amount)
    {
        BookId = bookId;
        Quantity = quantity;
        Amount = amount;

        ValidateOrderItemDomain();
    }

    private void ValidateOrderItemDomain()
    {
        DomainException.When(
            string.IsNullOrEmpty(BookId.ToString()),
            "A order item must have a book."
        );

        DomainException.When(
            Quantity <= 0,
            "Invalid item quantity, a valid quantity could not be 0 or less."
        );
    }

    public void UpdateAmount(double CurrentBookPrice)
    {
        Amount = CurrentBookPrice * Quantity;
    }
}
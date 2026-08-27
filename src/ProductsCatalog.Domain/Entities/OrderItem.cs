using ProductsCatalog.Domain.Exceptions;

namespace ProductsCatalog.Domain.Entities;

/// <summary>
/// Item de um pedido. Nao e um Aggregate Root: so existe dentro do ciclo de
/// vida de um <see cref="Order"/>. O preco unitario e "congelado" no momento
/// da compra (nao muda se o preco do livro mudar depois).
/// </summary>
public sealed class OrderItem
{
    public Guid BookId { get; private set; }
    public int Quantity { get; private set; }
    public double UnitPrice { get; private set; }

    public double Amount => UnitPrice * Quantity;

    private OrderItem()
    {
    }

    public OrderItem(Guid bookId, int quantity, double unitPrice)
    {
        BookId = bookId;
        Quantity = quantity;
        UnitPrice = unitPrice;

        Validate();
    }

    private void Validate()
    {
        DomainException.When(BookId == Guid.Empty, "An order item must reference a book.");
        DomainException.When(Quantity <= 0, "Invalid item quantity, must be greater than 0.");
        DomainException.When(UnitPrice < 0, "Unit price cannot be negative.");
    }
}

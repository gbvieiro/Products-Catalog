using Products.Catalog.Domain.Validations;
using Shared.Entities;

namespace Products.Catalog.Domain.Entities;

public class Stock : Entity
{
    public int Quantity { get; private set; }
    public Guid BookId { get; private set; }

    protected Stock()
    {
        Quantity = 0;
        BookId = Guid.Empty;
    }

    public Stock(int quantity, Guid bookId)
    {
        Quantity = quantity;
        BookId = bookId;

        ValidateDomain();
    }

    private void ValidateDomain()
    {
        DomainException.When(Id == Guid.Empty, "ID is required");
        DomainException.When(Quantity < 0, "Stock quantity invalid, could not be less than 0.");
        DomainException.When(BookId == Guid.Empty, "A book id is required.");
    }

    public void ReserveItems(int numberOfItems)
    {
        DomainException.When(numberOfItems > Quantity, $"Not enough stock. Available: {Quantity}.");
        Quantity -= numberOfItems;
    }

    public void AddBooksToStock(int numberOfItems)
    {
        Quantity += numberOfItems;
    }
}
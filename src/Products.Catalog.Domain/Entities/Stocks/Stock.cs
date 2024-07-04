using Products.Catalog.Domain.Entities.Base;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Stocks
{
    public sealed class Stock : IEntity<Guid>
    {
        public Guid Id { get; private set; }

        public int Quantity { get; private set; }

        public Guid BookId { get; private set; }

        public Stock(Guid id, int quantity, Guid bookId)
        {
            Id = id;
            Quantity = quantity;
            BookId = bookId;

            ValidateDomain();
        }

        private void ValidateDomain()
        {
            DomainExceptionValidation.When(Id == Guid.Empty, "ID is required");

            DomainExceptionValidation.When(Quantity < 0, "Stock quantity invalid, could not be less than 0.");

            DomainExceptionValidation.When(BookId == Guid.Empty, "A book id is required.");
        }

        public void ReserveItems(int numberOfItems)
        {
            DomainExceptionValidation.When(numberOfItems > Quantity, $"Not enough stock. Available: {Quantity}.");
            Quantity -= numberOfItems;
        }

        public void AddBooksToStock(int numberOfItems)
        {
            Quantity += numberOfItems;
        }

        public bool Equals(IEntity<Guid>? other)
        {
            return other is Stock && Id == other.Id;
        }
    }
}
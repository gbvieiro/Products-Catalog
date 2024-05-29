using Products.Catalog.Domain.Entities.Base;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Stocks
{
    /// <summary>
    /// Represents a product stock in the product catalog.
    /// </summary>
    public class Stock : IEntity<Guid>
    {
        /// <summary>
        /// A stock unique identificator.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The quantity of the product in stock.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// A book id.
        /// </summary>
        public Guid BookId { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">A stock unique identificator.</param>
        /// <param name="quantity">The quantity of the product in stock.</param>
        /// <param name="bookId">A book id.</param>
        public Stock(Guid id, int quantity, Guid bookId)
        {
            Id = id;
            Quantity = quantity;
            BookId = bookId;

            ValidateDomain();
        }

        /// <summary>
        /// Define rules for a valid Stock
        /// </summary>
        private void ValidateDomain()
        {
            // Id
            DomainExceptionValidation.When(Id == Guid.Empty, "ID is required");

            // Quantity
            DomainExceptionValidation.When(Quantity < 0, "Stock quantity invalid, could not be less than 0.");

            // Author
            DomainExceptionValidation.When(BookId == Guid.Empty, "A book id is required.");
        }

        /// <summary>
        /// Reserve items of from this Stock.
        /// </summary>
        /// <param name="numberOfItems">Number of items to reserve.</param>
        public void ReserveItems(int numberOfItems)
        {
            DomainExceptionValidation.When(numberOfItems > Quantity, $"Not enough stock. Available: {Quantity}.");
            Quantity -= numberOfItems;
        }

        /// <summary>
        /// Add items to this stock.
        /// </summary>
        /// <param name="numberOfItems">Number of items to add.</param>
        public void AddBooksToStock(int numberOfItems)
        {
            Quantity += numberOfItems;
        }

        /// <inheritdoc/>
        public bool Equals(IEntity<Guid>? other)
        {
            return other is Stock && Id == other.Id;
        }
    }
}
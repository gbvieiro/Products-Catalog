using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Orders
{
    /// <summary>
    /// Represents a order item in the product catalog.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// A book id.
        /// </summary>
        public Guid BookId { get; private set; }

        /// <summary>
        /// A quantity of the informed book.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Total amount for this item (current book price X quantity).
        /// </summary>
        public double Amount { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="bookId">A book id.</param>
        /// <param name="quantity">A quantity of the informed book.</param>
        /// <param name="amount">Total amount for this item (current book price X quantity).</param>
        public OrderItem(Guid bookId, int quantity, double amount)
        {
            BookId = bookId;
            Quantity = quantity;
            Amount = amount;

            ValidateOrderItemDomain();
        }

        /// <summary>
        /// Define rules for a valid book.
        /// </summary>
        private void ValidateOrderItemDomain()
        {
            // Id
            DomainExceptionValidation.When(
                string.IsNullOrEmpty(BookId.ToString()),
                "A order item must have a book."
            );

            // Items
            DomainExceptionValidation.When(
                Quantity <= 0,
                "Invalid item quantity, a valid quantity could not be 0 or less."
            );
        }

        /// <summary>
        /// Updated amount based on the current book price.
        /// </summary>
        /// <param name="CurrentBookPrice">Current book price.</param>
        public void UpdateAmount(double CurrentBookPrice)
        {
            Amount = CurrentBookPrice * Quantity;
        }
    }
}
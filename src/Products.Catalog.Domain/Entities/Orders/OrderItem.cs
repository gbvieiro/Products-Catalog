using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Orders
{
    public class OrderItem
    {
        public Guid BookId { get; private set; }
        public int Quantity { get; private set; }
        public double Amount { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="bookId">A book id.</param>
        /// <param name="quantity">Number of items.</param>
        /// <param name="amount">Amount of book values.</param>
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
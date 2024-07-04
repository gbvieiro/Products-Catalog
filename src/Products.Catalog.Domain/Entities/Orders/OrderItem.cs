using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Orders
{
    public sealed class OrderItem
    {
        public Guid BookId { get; private set; }

        public int Quantity { get; private set; }

        public double Amount { get; private set; }

        public OrderItem(Guid bookId, int quantity, double amount)
        {
            BookId = bookId;
            Quantity = quantity;
            Amount = amount;

            ValidateOrderItemDomain();
        }

        private void ValidateOrderItemDomain()
        {
            DomainExceptionValidation.When(
                string.IsNullOrEmpty(BookId.ToString()),
                "A order item must have a book."
            );

            DomainExceptionValidation.When(
                Quantity <= 0,
                "Invalid item quantity, a valid quantity could not be 0 or less."
            );
        }

        public void UpdateAmount(double CurrentBookPrice)
        {
            Amount = CurrentBookPrice * Quantity;
        }
    }
}
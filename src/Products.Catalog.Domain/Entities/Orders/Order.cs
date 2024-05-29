using Products.Catalog.Domain.Entities.Base;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Orders
{
    /// <summary>
    /// Represents a order in the product catalog.
    /// </summary>
    public class Order : IEntity<Guid>
    {
        /// <summary>
        /// A Order unique identificator.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// A customer ID.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// A creation data of this order.
        /// </summary>
        public DateTime CreationData { get; private set; }

        /// <summary>
        /// Current order status.
        /// </summary>
        public OrderStatusEnum Status { get; private set; }

        /// <summary>
        /// Items that are included in this order.
        /// </summary>
        public List<OrderItem> Items { get; private set; }

        /// <summary>
        /// Amount of values for this order.
        /// </summary>
        public double TotalAmount { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">A Order unique identificator.</param>
        /// <param name="customerId">A customer ID.</param>
        /// <param name="creationData">A creation data of this order.</param>
        /// <param name="status">Current order status.</param>
        /// <param name="items">Items that are included in this order.</param>
        /// <param name="totalAmount">Amount of values for this order.</param>
        public Order(
            Guid id,
            Guid customerId,
            DateTime creationData,
            OrderStatusEnum status,
            List<OrderItem> items,
            double totalAmount
        )
        {
            Id = id;
            CustomerId = customerId;
            CreationData = creationData;
            Status = status;
            Items = items;
            TotalAmount = totalAmount;

            ValidateBookDomain();
        }

        /// <summary>
        /// Define rules for a valid book.
        /// </summary>
        private void ValidateBookDomain()
        {
            // Id
            DomainExceptionValidation.When(Id == Guid.Empty, "ID is required");

            // CustomerId
            DomainExceptionValidation.When(CustomerId == Guid.Empty, "Custumer ID is required");

            // Items
            DomainExceptionValidation.When(Items == null || !Items.Any(), "A order must have order items.");
        }

        /// <summary>
        /// Set a new status for the order.
        /// </summary>
        public void SetStatus(OrderStatusEnum status)
        {
            this.Status = status;
        }

        /// <summary>
        /// Recalcule the toral ammount.
        /// </summary>
        public void UpdateTotalAmount()
        {
            TotalAmount = Items.Sum(x => x.Amount);
        }

        /// <summary>
        /// All entities must be able to verify equality. 
        /// Entities must be unique items.
        /// </summary>
        /// <param name="other">Other entity to compare.</param>
        /// <returns>True if objects are the same entity.</returns>
        public bool Equals(IEntity<Guid>? other)
        {
            return other is Order && other.Id == Id;
        }
    }
}
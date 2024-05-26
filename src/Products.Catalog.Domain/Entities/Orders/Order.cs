using Products.Catalog.Domain.Entities.Base;

namespace Products.Catalog.Domain.Entities.Orders
{
    /// <summary>
    /// Represents a order in the product catalog.
    /// </summary>
    public class Order(
        Guid id, DateTime creationData, OrderStatusEnum status,
        List<OrderItem> items, double totalAmount
    ) : IEntity<Guid>
    {
        /// <summary>
        /// A Order unique identificator.
        /// </summary>
        public Guid Id { get; private set; } = id;

        /// <summary>
        /// A creation data of this order.
        /// </summary>
        public DateTime CreationData { get; private set; } = creationData;

        /// <summary>
        /// Current order status.
        /// </summary>
        public OrderStatusEnum Status { get; private set; } = status;

        /// <summary>
        /// Items that are included in this order.
        /// </summary>
        public List<OrderItem> Items { get; private set; } = items;

        /// <summary>
        /// Amount of values for this order.
        /// </summary>
        public double TotalAmount { get; private set; } = totalAmount;

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
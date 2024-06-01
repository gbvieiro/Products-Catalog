using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.DTOs
{
    /// <summary>
    /// Represents a order structure.
    /// </summary>
    public class OrderDto : EntityDTO
    {
        /// <summary>
        /// A customer ID.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// A creation data of this order.
        /// </summary>
        public required DateTime CreationData { get; set; }

        /// <summary>
        /// Current order status.
        /// </summary>
        public required int Status { get; set; }

        /// <summary>
        /// Items that are included in this order.
        /// </summary>
        public required List<OrderItemDto> Items { get; set; }

        /// <summary>
        /// Amount of values for this order.
        /// </summary>
        public required double TotalAmount { get; set; }
    }

    /// <summary>
    /// Represents a order item structure.
    /// </summary>
    public class OrderItemDto
    {
        /// <summary>
        /// A book id.
        /// </summary>
        public Guid BookId { get; set; }

        /// <summary>
        /// A quantity of this item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The total amount. (this is calculated during order process)
        /// </summary>
        public double Amount { get; set; }
    }
}
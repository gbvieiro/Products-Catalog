using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.DTOs
{
    public class OrderDTO : EntityDTO
    {
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

    public class OrderItemDto
    {
        public Guid BookId { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
    }
}
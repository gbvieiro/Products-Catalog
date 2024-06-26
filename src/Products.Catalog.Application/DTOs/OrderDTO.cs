using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.DTOs
{
    public class OrderDto : EntityDTO
    {
        public Guid CustomerId { get; set; }

        public required DateTime CreationData { get; set; }

        public required int Status { get; set; }

        public required List<OrderItemDto> Items { get; set; }

        public required double TotalAmount { get; set; }
    }

    public class OrderItemDto
    {
        public Guid BookId { get; set; }

        public int Quantity { get; set; }

        public double Amount { get; set; }
    }
}
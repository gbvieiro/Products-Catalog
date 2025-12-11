using Application.DTOs;

namespace Products.Catalog.Application.DTOs;

public class OrderDto : EntityDto
{
    public Guid CustomerId { get; set; }

    public required DateTime CreationData { get; set; }

    public required int Status { get; set; }

    public required List<OrderItemDto> Items { get; set; }

    public required double TotalAmount { get; set; }
}
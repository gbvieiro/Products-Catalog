namespace Products.Catalog.Application.DTOs;

public class OrderItemDto
{
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
    public double Amount { get; set; }
}

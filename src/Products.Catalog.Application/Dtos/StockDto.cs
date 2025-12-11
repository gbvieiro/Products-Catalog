namespace Products.Catalog.Application.DTOs;

public class StockDto : EntityDto
{
    public int Quantity { get; set; }

    public Guid BookId { get; set; }
}
namespace Products.Catalog.Application.DTOs;

public class CompleteStockDto : StockDto
{
    public required BookDto Book { get; set; }
}
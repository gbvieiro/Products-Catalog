namespace Products.Catalog.Application.DTOs.Stocks
{
    public class CompleteStockDto : StockDto
    {
        public required BookDto Book { get; set; }
    }
}
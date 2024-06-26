using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.DTOs.Stocks
{
    public class StockDto : EntityDTO
    {
        public int Quantity { get; set; }

        public Guid BookId { get; set; }
    }
}
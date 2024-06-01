using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.DTOs.Stocks
{
    /// <summary>
    /// Represents a stock structure.
    /// </summary>
    public class StockDto : EntityDTO
    {
        /// <summary>
        /// The quantity of the product in stock.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// A book id.
        /// </summary>
        public Guid BookId { get; set; }
    }
}
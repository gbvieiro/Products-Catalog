using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.DTOs
{
    /// <summary>
    /// Represents a stock structure.
    /// </summary>
    public class StockDto : EntityDTO
    {
        /// <summary>
        /// A stock unique identificator.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The quantity of the product in stock.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// A book id.
        /// </summary>
        public Guid BookId { get; private set; }
    }
}
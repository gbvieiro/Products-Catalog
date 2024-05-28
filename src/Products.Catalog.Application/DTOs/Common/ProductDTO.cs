namespace Products.Catalog.Application.DTOs.Common
{
    public abstract class ProductDTO : EntityDTO
    {
        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        public required double Price { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product in stock.
        /// </summary>
        public required int StockQuantity { get; set; }
    }
}
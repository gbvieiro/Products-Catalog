namespace Products.Catalog.Application.DTOs.Common
{
    /// <summary>
    /// Represents a product structure.
    /// </summary>
    public abstract class ProductDTO : EntityDTO
    {
        /// <summary>
        /// Price of the product.
        /// </summary>
        public required double Price { get; set; }
    }
}
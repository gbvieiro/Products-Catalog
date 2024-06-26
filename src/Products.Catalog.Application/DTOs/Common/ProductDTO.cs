namespace Products.Catalog.Application.DTOs.Common
{
    public abstract class ProductDTO : EntityDTO
    {
        public required double Price { get; set; }
    }
}
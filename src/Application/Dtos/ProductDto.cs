namespace Products.Catalog.Application.DTOs;

public abstract class ProductDto : EntityDto
{
    public required double Price { get; set; }
}
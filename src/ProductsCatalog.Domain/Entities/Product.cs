using ProductsCatalog.Domain.Common;

namespace ProductsCatalog.Domain.Entities;

/// <summary>Base para qualquer entidade vendavel do catalogo (hoje apenas Book).</summary>
public abstract class Product : BaseEntity
{
    protected Product()
    {
    }

    protected Product(double price)
    {
        Price = price;
    }

    public double Price { get; protected set; }
}

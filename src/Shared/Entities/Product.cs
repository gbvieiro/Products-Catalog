namespace Shared.Entities;

public abstract class Product(double price) : Entity
{
    // Construtor sem parâmetros para o Entity Framework Core
    protected Product() : this(0)
    {
    }

    public double Price { get; protected set; } = price;
}
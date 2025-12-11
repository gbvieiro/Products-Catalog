using Products.Catalog.Domain.Entities;

namespace Products.Catalog.Domain.Tests;

public static class TestHelpers
{
    public static List<OrderItem> CreateValidOrderItems(Guid bookId, int quantity = 1, double price = 29.99)
    {
        return new List<OrderItem>
        {
            new OrderItem(bookId, quantity, price * quantity)
        };
    }
}


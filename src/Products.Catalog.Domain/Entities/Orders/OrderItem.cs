namespace Products.Catalog.Domain.Entities.Orders
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }
        public decimal Amount { get; private set; }
    }
}
using Products.Catalog.Domain.Entities.Orders;

namespace Products.Catalog.Domain.Services.Stock
{
    public interface IOrderDomainService
    {
        Task ProcessNewOrderAsync(Order order);

        Task CancelOrderAsync(Order order);
    }
}
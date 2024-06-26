using Products.Catalog.Domain.Entities.Orders;

namespace Products.Catalog.Domain.Services.Orders
{
    public interface IOrderDomainService
    {
        Task ProcessNewOrderAsync(Order order);

        Task CancelOrderAsync(Order order);
    }
}
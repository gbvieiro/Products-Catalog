using Products.Catalog.Domain.Entities.Orders;

namespace Products.Catalog.Domain.Services.Stock
{
    public interface IOrderDomainService
    {
        void ProcessNewOrder(Order order);

        void CancelOrder(Order order);
    }
}
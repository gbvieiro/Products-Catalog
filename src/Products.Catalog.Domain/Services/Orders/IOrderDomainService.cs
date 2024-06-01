using Products.Catalog.Domain.Entities.Orders;

namespace Products.Catalog.Domain.Services.Orders
{
    /// <summary>
    /// A domain service that define important busines rules for orders.
    /// </summary>
    public interface IOrderDomainService
    {
        /// <summary>
        /// This method process a new order.
        /// </summary>
        /// <param name="order">A order.</param>
        Task ProcessNewOrderAsync(Order order);

        /// <summary>
        /// This method will cancel a order.
        /// </summary>
        /// <param name="order">A order.</param>
        Task CancelOrderAsync(Order order);
    }
}
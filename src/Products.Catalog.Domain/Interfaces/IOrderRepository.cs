using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.Interfaces.Common;

namespace Products.Catalog.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        Task<IEnumerable<Order>> GetAllAsync(string filter, int skip, int take, Guid userId);
    }
}

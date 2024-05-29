using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces.Common;

namespace Products.Catalog.Domain.RepositoriesInterfaces
{
    /// <summary>
    /// A interface for a orders repository.
    /// </summary>
    public interface IOrderRepository : IRepository<Order, Guid>
    {
    }
}

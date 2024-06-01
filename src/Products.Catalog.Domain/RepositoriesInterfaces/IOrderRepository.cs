using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces.Common;

namespace Products.Catalog.Domain.RepositoriesInterfaces
{
    /// <summary>
    /// A interface for a orders repository.
    /// </summary>
    public interface IOrderRepository : IRepository<Order, Guid>
    {
        /// <summary>
        /// Get all user entities paginated.
        /// </summary>
        /// <param name="filter">A filter text.</param>
        /// <param name="skip">A number of items to skip.</param>
        /// <param name="take">A number of items to take.</param>
        /// <param name="userId">A user id as string.</param>
        /// <returns>A list of entities.</returns>
        Task<IEnumerable<Order>> GetAllAsync(string filter, int skip, int take, Guid userId);
    }
}

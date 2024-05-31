using Product.Catalog.Infra.Data.Database;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Product.Catalog.Infra.Data.Repositories
{
    /// <summary>
    /// A order repository definition.
    /// </summary>
    public class OrdersRepository : IOrderRepository
    {
        /// <inheritdoc/>
        public Task DeleteAsync(Guid id)
        {
            Context.Orders = new List<Order>(Context.Orders.Where(x => x.Id !=id));

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<Order?> GetAsync(Guid id)
        {
            return Task.FromResult(Context.Orders.FirstOrDefault(x => x.Id == id));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Order>> GetAllAsync(string filter, int skip, int take)
        {
            if (Context.Orders == null)
            {
                throw new Exception("Context is not initialized.");
            }

            if (string.IsNullOrWhiteSpace(filter))
            {
                var orders = Context.Orders.Where(
                    x => x.Status.ToString().Contains(filter) &&
                         x.Id.ToString().Contains(filter)
                ).Skip(skip).Take(take);

                Task.FromResult(orders);
            }

            return Task.FromResult(Context.Orders.Skip(skip).Take(take));
        }

        /// <inheritdoc/>
        public Task SaveAsync(Order entity)
        {
            var savedEntityIndex = Context.Orders.FindIndex(x => x.Id == entity.Id);

            if (savedEntityIndex != -1) {
                Context.Orders.Add(entity);
            } else {
                Context.Orders[savedEntityIndex] = entity;
            }

            return Task.CompletedTask;
        }
    }
}
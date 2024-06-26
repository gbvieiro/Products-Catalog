using Product.Catalog.Infra.Data.Database;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Product.Catalog.Infra.Data.Repositories
{
    public class OrdersRepository : IOrderRepository
    {
        public Task DeleteAsync(Guid id)
        {
            Context.Orders = new List<Order>(Context.Orders.Where(x => x.Id !=id));

            return Task.CompletedTask;
        }

        public Task<Order?> GetAsync(Guid id)
        {
            return Task.FromResult(Context.Orders.FirstOrDefault(x => x.Id == id));
        }

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

        public Task SaveAsync(Order entity)
        {
            var savedEntityIndex = Context.Orders.FindIndex(x => x.Id == entity.Id);

            if (savedEntityIndex == -1) {
                Context.Orders.Add(entity);
            } else {
                Context.Orders[savedEntityIndex] = entity;
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Order>> GetAllAsync(string filter, int skip, int take, Guid userId)
        {
            if (Context.Orders == null)
            {
                throw new Exception("Context is not initialized.");
            }

            if (string.IsNullOrWhiteSpace(filter))
            {
                return Task.FromResult(Context.Orders.Where(x =>
                    x.CustomerId == userId &&
                    x.Status.ToString().Contains(filter) ||
                    x.Id.ToString().Contains(filter) 
                ).Skip(skip).Take(take));
            }

            return Task.FromResult(Context.Orders.Where(x => x.CustomerId == userId).Skip(skip).Take(take));
        }
    }
}
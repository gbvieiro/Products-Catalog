using Product.Catalog.Infra.Data.Database;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Product.Catalog.Infra.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public void Delete(Guid id)
        {
            Context.Orders = new List<Order>(Context.Orders.Where(x => x.Id !=id));
        }

        public Order? Get(Guid id)
        {
            return Context.Orders.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Order> GetAll(string filter, int skip, int take)
        {
            if (Context.Orders == null)
            {
                throw new Exception("Context is not initialized.");
            }

            if (string.IsNullOrWhiteSpace(filter))
            {
                return Context.Orders.Where(
                    x => x.Status.ToString().Contains(filter) &&
                         x.Id.ToString().Contains(filter)
                ).Skip(skip).Take(take).ToList();
            }

            return Context.Orders.Skip(skip).Take(take).ToList();
        }

        public void Save(Order entity)
        {
            var savedEntityIndex = Context.Orders.FindIndex(x => x.Id == entity.Id);

            if (savedEntityIndex != -1) {
                Context.Orders.Add(entity);
            } else {
                Context.Orders[savedEntityIndex] = entity;
            }
        }
    }
}
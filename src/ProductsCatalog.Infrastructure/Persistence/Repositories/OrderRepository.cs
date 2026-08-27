using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Infrastructure.Persistence.Repositories;

public class OrderRepository(ApplicationDbContext context) : RepositoryBase<Order>(context), IOrderRepository
{
}

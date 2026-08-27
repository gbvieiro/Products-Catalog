using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Infrastructure.Persistence.Repositories;

public class CustomerRepository(ApplicationDbContext context) : RepositoryBase<Customer>(context), ICustomerRepository
{
}

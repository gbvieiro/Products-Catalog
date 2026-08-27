using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Infrastructure.Persistence.Repositories;

public class BookRepository(ApplicationDbContext context) : RepositoryBase<Book>(context), IBookRepository
{
}

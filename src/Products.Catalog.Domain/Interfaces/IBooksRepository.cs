using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Interfaces.Common;

namespace Products.Catalog.Domain.Interfaces
{
    public interface IBooksRepository : IRepository<Book, Guid> 
    {
        Task<double> GetBookPrice(Guid id);
    }
}
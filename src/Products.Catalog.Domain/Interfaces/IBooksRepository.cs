using Products.Catalog.Domain.Entities;

namespace Products.Catalog.Domain.Interfaces
{
    public interface IBooksRepository : IRepository<Book> 
    {
        Task<double> GetBookPrice(Guid id);
    }
}
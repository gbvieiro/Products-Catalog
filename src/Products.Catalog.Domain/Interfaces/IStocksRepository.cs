using Products.Catalog.Domain.Entities;

namespace Products.Catalog.Domain.Interfaces
{
    public interface IStocksRepository : IRepository<Stock>
    {
        Task<Stock?> GetAsync(Guid id);
        Task<IEnumerable<Stock>> GetAllAsync(string filter, int skip, int take);
        Task SaveAsync(Stock entity);
        Task<Stock?> GetByBookId(Guid bookId);
    }
}
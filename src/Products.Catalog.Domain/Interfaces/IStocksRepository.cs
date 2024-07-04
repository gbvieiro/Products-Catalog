using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.Interfaces.Common;

namespace Products.Catalog.Domain.Interfaces
{
    public interface IStocksRepository : IRepository<Stock, Guid>
    {
        Task<Stock?> GetByBookId(Guid bookId);
    }
}
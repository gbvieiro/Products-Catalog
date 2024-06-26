using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.RepositoriesInterfaces.Common;

namespace Products.Catalog.Domain.RepositoriesInterfaces
{
    public interface IStocksRepository : IRepository<Stock, Guid>
    {
        Task<Stock?> GetByBookId(Guid bookId);
    }
}
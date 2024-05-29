using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.RepositoriesInterfaces.Common;

namespace Products.Catalog.Domain.RepositoriesInterfaces
{
    public interface IStocksRepository : IRepository<Stock, Guid>
    {
        /// <summary>
        /// Get Stock by book id.
        /// </summary>
        /// <param name="bookId">A book id.</param>
        /// <returns>A stock when available.</returns>
        Task<Stock?> GetByBookId(Guid bookId);
    }
}
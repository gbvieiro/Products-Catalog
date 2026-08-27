using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Domain.Repositories;

public interface IStockRepository : IRepository<Stock>
{
    Task<Stock?> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);
}

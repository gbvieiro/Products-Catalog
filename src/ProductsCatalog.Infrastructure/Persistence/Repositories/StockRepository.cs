using Microsoft.EntityFrameworkCore;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Infrastructure.Persistence.Repositories;

public class StockRepository(ApplicationDbContext context) : RepositoryBase<Stock>(context), IStockRepository
{
    public async Task<Stock?> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default) =>
        await DbSet.FirstOrDefaultAsync(s => s.BookId == bookId, cancellationToken);
}

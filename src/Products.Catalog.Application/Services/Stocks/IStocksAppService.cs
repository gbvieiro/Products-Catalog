using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Stocks;

public interface IStocksAppService : ICrudAppService<StockDto>
{
    Task<CompleteStockDto?> GetStockByBookId(Guid bookId);

    Task<string> AddItemsToStock(Guid bookId, int quantity);
}
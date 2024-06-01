using Products.Catalog.Application.DTOs.Stocks;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Stocks
{
    /// <summary>
    /// Provide a interface for stock domain user cases.
    /// </summary>
    public interface IStocksAppService : ICrudAppService<StockDto>
    {
        /// <summary>
        /// Get a stock by a book id.
        /// </summary>
        /// <param name="bookId">A book id.</param>
        /// <returns>A complete stock DTO.</returns>
        Task<CompleteStockDto?> GetStockByBookId(Guid bookId);

        /// <summary>
        /// Add items to a stock.
        /// </summary>
        /// <param name="bookId">A book id.</param>
        /// <param name="quantity">A book quantity.</param>
        /// <returns>A response message.</returns>
        Task<string> AddItemsToStock(Guid bookId, int quantity);
    }
}
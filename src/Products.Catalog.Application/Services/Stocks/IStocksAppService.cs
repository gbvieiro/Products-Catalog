using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Stocks
{
    /// <summary>
    /// Provide a interface for stock domain user cases.
    /// </summary>
    public interface IStocksAppService : ICrudAppService<StockDto>
    {
    }
}
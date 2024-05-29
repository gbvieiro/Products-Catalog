using Products.Catalog.Application.DTOs;

namespace Products.Catalog.Application.Services.Stocks
{
    public class StocksAppService : IStocksAppService
    {
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<StockDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<StockDto?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(StockDto book)
        {
            throw new NotImplementedException();
        }
    }
}

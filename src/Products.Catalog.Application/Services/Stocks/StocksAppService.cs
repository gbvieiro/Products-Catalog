using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Products.Catalog.Application.Services.Stocks
{
    /// <summary>
    /// Provide stock domain user cases.
    /// </summary>
    public class StocksAppService(IStocksRepository stocksRepository, IMapper mapper) : IStocksAppService
    {
        /// <summary>
        /// A order repository interface.
        /// </summary>
        private readonly IStocksRepository _stocksRepository = stocksRepository;

        /// <summary>
        /// A mapper service.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc/>
        public Task DeleteAsync(Guid id) => _stocksRepository.DeleteAsync(id);

        /// <inheritdoc/>
        public async Task<List<StockDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            var stocks = await _stocksRepository.GetAllAsync(filtertext, skip, take);
            return _mapper.Map<List<StockDto>>(stocks.ToList());
        }

        /// <inheritdoc/>
        public async Task<StockDto?> GetAsync(Guid id)
        {
            var stock = await _stocksRepository.GetAsync(id);
            return stock != null ? _mapper.Map<StockDto>(stock) : default;
        }

        /// <inheritdoc/>
        public Task SaveAsync(StockDto stockDto)
        {
            ArgumentNullException.ThrowIfNull(stockDto);
            stockDto.GenerateId();
            var stock = _mapper.Map<Stock>(stockDto);
            return _stocksRepository.SaveAsync(stock);
        }
    }
}

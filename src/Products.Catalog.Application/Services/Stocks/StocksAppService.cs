using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Stocks;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Products.Catalog.Application.Services.Stocks
{
    /// <summary>
    /// Provide stock domain user cases.
    /// </summary>
    /// <param name="stocksRepository">A stocks repository instance.</param>
    /// <param name="booksRepository">A books repository instance.</param>
    /// <param name="mapper">A mapper service instance.</param>
    public class StocksAppService(
        IStocksRepository stocksRepository,
        IBooksRepository booksRepository,
        IMapper mapper
    ) : IStocksAppService
    {
        /// <summary>
        /// A order repository interface.
        /// </summary>
        private readonly IStocksRepository _stocksRepository = stocksRepository;

        /// <summary>
        /// A book repository interface
        /// </summary>
        private readonly IBooksRepository _booksRepository = booksRepository;

        /// <summary>
        /// A mapper service.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc/>
        public async Task<string> AddItemsToStock(Guid bookId, int quantity)
        {
            var stock = await _stocksRepository.GetByBookId(bookId);
            
            if (stock == null)
            {
                return $"Stock for book {bookId} could not be found.";
            }

            stock.AddBooksToStock(quantity);

            await _stocksRepository.SaveAsync(stock);

            return $"Stock updated! Available items: {stock.Quantity}";
        }

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

        public async Task<CompleteStockDto?> GetStockByBookId(Guid bookId)
        {
            var stock = await _stocksRepository.GetByBookId(bookId);

            if(stock == null)
            {
                return null;
            }

            var book = await _booksRepository.GetAsync(bookId);

            if(book == null)
            {
                return null;
            }

            return new CompleteStockDto() { 
                BookId  = stock.BookId,
                Quantity = stock.Quantity,
                Id = stock.Id,
                Book = _mapper.Map<BookDto>(book)
            };
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

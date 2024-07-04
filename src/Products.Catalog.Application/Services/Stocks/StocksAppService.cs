using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Stocks;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.Interfaces;

namespace Products.Catalog.Application.Services.Stocks
{
    public class StocksAppService(
        IStocksRepository stocksRepository,
        IBooksRepository booksRepository,
        IMapper mapper
    ) : IStocksAppService
    {
        private readonly IStocksRepository _stocksRepository = stocksRepository;

        private readonly IBooksRepository _booksRepository = booksRepository;

        private readonly IMapper _mapper = mapper;

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

        public Task DeleteAsync(Guid id) => _stocksRepository.DeleteAsync(id);

        public async Task<List<StockDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            var stocks = await _stocksRepository.GetAllAsync(filtertext, skip, take);
            return _mapper.Map<List<StockDto>>(stocks.ToList());
        }

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

        public Task SaveAsync(StockDto stockDto)
        {
            ArgumentNullException.ThrowIfNull(stockDto);
            stockDto.GenerateId();
            var stock = _mapper.Map<Stock>(stockDto);
            return _stocksRepository.SaveAsync(stock);
        }
    }
}
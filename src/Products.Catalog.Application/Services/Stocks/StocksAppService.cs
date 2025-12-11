using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Stocks;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Interfaces;

namespace Products.Catalog.Application.Services.Stocks
{
    public class StocksAppService(
        IRepository<Stock> stocksRepository,
        IRepository<Book> booksRepository,
        IMapper mapper
    ) : IStocksAppService
    {
        private readonly IRepository<Stock> _stocksRepository = stocksRepository;

        private readonly IRepository<Book> _booksRepository = booksRepository;

        private readonly IMapper _mapper = mapper;

        public async Task<string> AddItemsToStock(Guid bookId, int quantity)
        {
            var stocks = await _stocksRepository.FindAsync(string.Empty, 0, int.MaxValue);
            var stock = stocks.FirstOrDefault(s => s.BookId == bookId);
            
            if (stock == null)
            {
                return $"Stock for book {bookId} could not be found.";
            }

            stock.AddBooksToStock(quantity);

            await _stocksRepository.UpdateAsync(stock.Id, stock);

            return $"Stock updated! Available items: {stock.Quantity}";
        }

        public Task DeleteAsync(Guid id) => _stocksRepository.DeleteAsync(id);

        public async Task<List<StockDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            var stocks = await _stocksRepository.FindAsync(filtertext, skip, take);
            return _mapper.Map<List<StockDto>>(stocks.ToList());
        }

        public async Task<StockDto?> GetAsync(Guid id)
        {
            var stock = await _stocksRepository.ReadAsync(id);
            return stock != null ? _mapper.Map<StockDto>(stock) : default;
        }

        public async Task<CompleteStockDto?> GetStockByBookId(Guid bookId)
        {
            var stocks = await _stocksRepository.FindAsync(string.Empty, 0, int.MaxValue);
            var stock = stocks.FirstOrDefault(s => s.BookId == bookId);

            if(stock == null)
            {
                return null;
            }

            var book = await _booksRepository.ReadAsync(bookId);

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

        public async Task SaveAsync(StockDto stockDto)
        {
            ArgumentNullException.ThrowIfNull(stockDto);
            stockDto.GenerateId();
            var stock = _mapper.Map<Stock>(stockDto);
            
            var existingStock = await _stocksRepository.ReadAsync(stock.Id);
            if (existingStock == null)
            {
                await _stocksRepository.CreateAsync(stock);
            }
            else
            {
                await _stocksRepository.UpdateAsync(stock.Id, stock);
            }
        }
    }
}
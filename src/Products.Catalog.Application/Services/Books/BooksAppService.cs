using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Interfaces;

namespace Products.Catalog.Application.Services.Books
{
    public class BooksAppService(
        IRepository<Book> bookRepository,
        IRepository<Stock> stocksRepository,
        IMapper mapper
    ) : IBooksAppService
    {
        private readonly IRepository<Book> _bookRepository = bookRepository;

        private readonly IRepository<Stock> _stocksRepository = stocksRepository;

        private readonly IMapper _mapper = mapper;

        public async Task DeleteAsync(Guid id)
        {
            var stocks = await _stocksRepository.FindAsync(string.Empty, 0, int.MaxValue);
            var stock = stocks.FirstOrDefault(s => s.BookId == id);

            var tasks = new List<Task>
            {
                _bookRepository.DeleteAsync(id)
            };

            if(stock != null)
            {
                tasks.Add(_stocksRepository.DeleteAsync(stock.Id));
            }

            // Execute all in parallel.
            await Task.WhenAll(tasks);
        }

        public async Task<List<BookDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            var books = await _bookRepository.FindAsync(filtertext, skip, take);
            return _mapper.Map<List<BookDto>>(books.ToList());
        }

        public async Task<BookDto?> GetAsync(Guid id)
        {
            var book = await _bookRepository.ReadAsync(id);
            return book != null ? _mapper.Map<BookDto>(book) : default;
        }

        public async Task SaveAsync(BookDto bookDto)
        {
            ArgumentNullException.ThrowIfNull(bookDto);

            bookDto.GenerateId();
            var book = _mapper.Map<Book>(bookDto);
            
            var existingBook = await _bookRepository.ReadAsync(book.Id);
            if (existingBook == null)
            {
                await _bookRepository.CreateAsync(book);
            }
            else
            {
                await _bookRepository.UpdateAsync(book.Id, book);
            }

            var newBookStock = new Stock(0, book.Id);

            var existingStock = (await _stocksRepository.FindAsync(string.Empty, 0, int.MaxValue))
                .FirstOrDefault(s => s.BookId == book.Id);
            
            if (existingStock == null)
            {
                await _stocksRepository.CreateAsync(newBookStock);
            }
            else
            {
                await _stocksRepository.UpdateAsync(existingStock.Id, newBookStock);
            }
        }
    }
}
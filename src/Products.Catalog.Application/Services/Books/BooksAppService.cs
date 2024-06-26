using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Products.Catalog.Application.Services.Books
{
    public class BooksAppService(
        IBooksRepository bookRepository,
        IStocksRepository stocksRepository,
        IMapper mapper
    ) : IBooksAppService
    {
        private readonly IBooksRepository _bookRepository = bookRepository;

        private readonly IStocksRepository _stocksRepository = stocksRepository;

        private readonly IMapper _mapper = mapper;

        public async Task DeleteAsync(Guid id)
        {
            var stock = await _stocksRepository.GetByBookId(id);

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
            var books = await _bookRepository.GetAllAsync(filtertext, skip, take);
            return _mapper.Map<List<BookDto>>(books.ToList());
        }

        public async Task<BookDto?> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);
            return book != null ? _mapper.Map<BookDto>(book) : default;
        }

        public async Task SaveAsync(BookDto bookDto)
        {
            ArgumentNullException.ThrowIfNull(bookDto);

            bookDto.GenerateId();
            var book = _mapper.Map<Book>(bookDto);
            
            await _bookRepository.SaveAsync(book);

            var newBookStock = new Stock(Guid.NewGuid(), 0, book.Id);

            await _stocksRepository.SaveAsync(newBookStock);
        }
    }
}
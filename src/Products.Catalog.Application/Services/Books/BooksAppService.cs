using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Products.Catalog.Application.Services.Books
{
    /// <summary>
    /// Provide book domain user cases.
    /// </summary>
    /// <param name="bookRepository">A book repository instance.</param>
    /// <param name="stocksRepository">A stock repository instance.</param>
    /// <param name="mapper">A mapper service.</param>
    public class BooksAppService(
        IBooksRepository bookRepository,
        IStocksRepository stocksRepository,
        IMapper mapper
    ) : IBooksAppService
    {
        /// <summary>
        /// A books repository interface.
        /// </summary>
        private readonly IBooksRepository _bookRepository = bookRepository;

        /// <summary>
        /// A stocks repository interface.
        /// </summary>
        private readonly IStocksRepository _stocksRepository = stocksRepository;

        /// <summary>
        /// A mapper service.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<List<BookDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            var books = await _bookRepository.GetAllAsync(filtertext, skip, take);
            return _mapper.Map<List<BookDto>>(books.ToList());
        }

        /// <inheritdoc/>
        public async Task<BookDto?> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);
            return book != null ? _mapper.Map<BookDto>(book) : default;
        }

        /// <inheritdoc/>
        public async Task SaveAsync(BookDto bookDto)
        {
            // Avoid bad requests.
            ArgumentNullException.ThrowIfNull(bookDto);

            bookDto.GenerateId();
            var book = _mapper.Map<Book>(bookDto);
            
            // Save book
            await _bookRepository.SaveAsync(book);

            // Creates a new book stock
            // All registeres books must have a stock.
            var newBookStock = new Stock(Guid.NewGuid(), 0, book.Id);

            // Save stock
            await _stocksRepository.SaveAsync(newBookStock);
        }
    }
}
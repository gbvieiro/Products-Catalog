using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Products.Catalog.Application.Services.Books
{
    /// <summary>
    /// Provide book domain user cases.
    /// </summary>
    /// <remarks>
    /// Constructor.
    /// </remarks>
    /// <param name="bookRepository">A book repository instace.</param>
    public class BookAppService(IBookRepository bookRepository, IMapper mapper) : IBookAppService
    {
        /// <summary>
        /// A book repository interface.
        /// </summary>
        private readonly IBookRepository _bookRepository = bookRepository;

        /// <summary>
        /// A mapper service.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc/>
        public Task DeleteAsync(Guid id) => _bookRepository.DeleteAsync(id);

        /// <inheritdoc/>
        public async Task<List<BookDTO>> GetAllAsync(string filtertext, int skip, int take)
        {
            var books = await _bookRepository.GetAllAsync(filtertext, skip, take);
            return _mapper.Map<List<BookDTO>>(books.ToList());
        }

        public async Task<BookDTO?> GetAsync(Guid id)
        {
            var book = await _bookRepository.GetAsync(id);
            return book != null ? _mapper.Map<BookDTO>(book) : default;
        }

        public Task SaveAsync(BookDTO bookDto)
        {
            ArgumentNullException.ThrowIfNull(bookDto);
            bookDto.GenerateId();
            var book = _mapper.Map<Book>(bookDto);
            return _bookRepository.SaveAsync(book);
        }
    }
}
using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Products.Catalog.Application.Services.Books
{
    /// <summary>
    /// Provide book domain user cases.
    /// </summary>
    public class BooksAppService(IBooksRepository bookRepository, IMapper mapper) : IBooksAppService
    {
        /// <summary>
        /// A book repository interface.
        /// </summary>
        private readonly IBooksRepository _bookRepository = bookRepository;

        /// <summary>
        /// A mapper service.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc/>
        public Task DeleteAsync(Guid id) => _bookRepository.DeleteAsync(id);

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
        public Task SaveAsync(BookDto bookDto)
        {
            ArgumentNullException.ThrowIfNull(bookDto);
            bookDto.GenerateId();
            var book = _mapper.Map<Book>(bookDto);
            return _bookRepository.SaveAsync(book);
        }
    }
}
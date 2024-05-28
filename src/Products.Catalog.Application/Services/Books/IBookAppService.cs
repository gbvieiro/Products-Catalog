using Products.Catalog.Application.DTOs;

namespace Products.Catalog.Application.Services.Books
{
    /// <summary>
    /// Provide access to book domain user cases.
    /// </summary>
    public interface IBookAppService
    {
        /// <summary>
        /// Get a book by id.
        /// </summary>
        /// <param name="id">A book id.</param>
        /// <returns>A book dto.</returns>
        Task<BookDTO?> GetAsync(Guid id);

        /// <summary>
        /// Get all books using pagination.
        /// </summary>
        /// <param name="filtertext">A filter text.</param>
        /// <param name="skip">A number of books to skip.</param>
        /// <param name="take">A number of books to take.</param>
        /// <returns>A list of books.</returns>
        Task<List<BookDTO>> GetAllAsync(string filtertext, int skip, int take);

        /// <summary>
        /// Save a book.
        /// </summary>
        /// <param name="book">A book.</param>
        Task SaveAsync(BookDTO book);

        /// <summary>
        /// Delete a book.
        /// </summary>
        /// <param name="book">A book id.</param>
        Task DeleteAsync(Guid id);
    }
}
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.RepositoriesInterfaces.Common;

namespace Products.Catalog.Domain.RepositoriesInterfaces
{
    public interface IBookRepository : IRepository<Book, Guid> 
    {
        /// <summary>
        /// Get a book price by id.
        /// </summary>
        /// <param name="id">A book id.</param>
        /// <returns>A book price.</returns>
        Task<double> GetBookPrice(Guid id);
    }
}
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Books
{
    /// <summary>
    /// Provide a interface for books domain user cases.
    /// </summary>
    public interface IBooksAppService : ICrudAppService<BookDto>
    {
        
    }
}
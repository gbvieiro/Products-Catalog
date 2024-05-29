using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Books
{
    /// <summary>
    /// Provide access to book domain user cases.
    /// </summary>
    public interface IBookAppService : IAppService<BookDto>
    {
        
    }
}
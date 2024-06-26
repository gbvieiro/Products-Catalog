using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Common;

namespace Products.Catalog.Application.Services.Books
{
    public interface IBooksAppService : ICrudAppService<BookDto> { }
}
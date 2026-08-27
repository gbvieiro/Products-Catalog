using ProductsCatalog.Application.Common.Messaging;
using ProductsCatalog.Application.Common.Models;

namespace ProductsCatalog.Application.Features.Books.Queries.GetBooks;

public sealed record GetBooksQuery(string? Filter = null, int Skip = 0, int Take = 20)
    : IQuery<PagedResult<BookDto>>;

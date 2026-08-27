using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Books.Queries.GetBookById;

public sealed record GetBookByIdQuery(Guid Id) : IQuery<BookDto?>;

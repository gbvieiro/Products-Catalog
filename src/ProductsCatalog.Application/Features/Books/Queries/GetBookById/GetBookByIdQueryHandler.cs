using AutoMapper;
using MediatR;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Books.Queries.GetBookById;

public sealed class GetBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper)
    : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken);
        return book is null ? null : mapper.Map<BookDto>(book);
    }
}

using AutoMapper;
using MediatR;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.Specifications;

namespace ProductsCatalog.Application.Features.Books.Queries.GetBooks;

public sealed class GetBooksQueryHandler(IBookRepository bookRepository, IMapper mapper)
    : IRequestHandler<GetBooksQuery, PagedResult<BookDto>>
{
    public async Task<PagedResult<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var spec = new BooksFilterSpecification(request.Filter, request.Skip, request.Take);

        var books = await bookRepository.ListAsync(spec, cancellationToken);
        var total = await bookRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<BookDto>(mapper.Map<List<BookDto>>(books), total, request.Skip, request.Take);
    }
}

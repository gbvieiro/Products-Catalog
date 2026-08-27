using AutoMapper;
using MediatR;
using ProductsCatalog.Application.Features.Books;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Stocks.Queries.GetStockByBookId;

public sealed class GetStockByBookIdQueryHandler(
    IStockRepository stockRepository,
    IBookRepository bookRepository,
    IMapper mapper) : IRequestHandler<GetStockByBookIdQuery, CompleteStockDto?>
{
    public async Task<CompleteStockDto?> Handle(GetStockByBookIdQuery request, CancellationToken cancellationToken)
    {
        var stock = await stockRepository.GetByBookIdAsync(request.BookId, cancellationToken);
        if (stock is null)
        {
            return null;
        }

        var book = await bookRepository.GetByIdAsync(request.BookId, cancellationToken);
        if (book is null)
        {
            return null;
        }

        return new CompleteStockDto
        {
            Id = stock.Id,
            BookId = stock.BookId,
            Quantity = stock.Quantity,
            Book = mapper.Map<BookDto>(book)
        };
    }
}

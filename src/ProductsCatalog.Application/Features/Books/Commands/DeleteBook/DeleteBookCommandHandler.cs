using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Books.Commands.DeleteBook;

public sealed class DeleteBookCommandHandler(
    IBookRepository bookRepository,
    IStockRepository stockRepository) : IRequestHandler<DeleteBookCommand, Unit>
{
    public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Book), request.Id);

        bookRepository.Remove(book);

        var stock = await stockRepository.GetByBookIdAsync(request.Id, cancellationToken);
        if (stock is not null)
        {
            stockRepository.Remove(stock);
        }

        return Unit.Value;
    }
}

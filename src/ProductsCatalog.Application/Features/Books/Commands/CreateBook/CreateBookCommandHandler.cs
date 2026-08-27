using MediatR;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Books.Commands.CreateBook;

/// <summary>Cria o livro e ja inicializa o estoque dele com quantidade zero.</summary>
public sealed class CreateBookCommandHandler(
    IBookRepository bookRepository,
    IStockRepository stockRepository) : IRequestHandler<CreateBookCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book(request.Price, request.Title, request.Author, request.Genre);
        await bookRepository.AddAsync(book, cancellationToken);

        var stock = new Stock(book.Id, quantity: 0);
        await stockRepository.AddAsync(stock, cancellationToken);

        return book.Id;
    }
}

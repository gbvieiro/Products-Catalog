using FluentAssertions;
using Moq;
using ProductsCatalog.Application.Features.Books.Commands.CreateBook;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.Repositories;
using Xunit;

namespace ProductsCatalog.Application.Tests.Features.Books;

public class CreateBookCommandHandlerTests
{
    private readonly Mock<IBookRepository> _bookRepository = new();
    private readonly Mock<IStockRepository> _stockRepository = new();

    [Fact]
    public async Task Handle_CreatesBookAndZeroedStock()
    {
        var handler = new CreateBookCommandHandler(_bookRepository.Object, _stockRepository.Object);
        var command = new CreateBookCommand(29.9, "Clean Architecture", "Robert C. Martin", EBookGenre.NonFiction);

        var bookId = await handler.Handle(command, CancellationToken.None);

        bookId.Should().NotBeEmpty();

        _bookRepository.Verify(r => r.AddAsync(
            It.Is<Book>(b => b.Title == "Clean Architecture" && b.Id == bookId),
            It.IsAny<CancellationToken>()), Times.Once);

        _stockRepository.Verify(r => r.AddAsync(
            It.Is<Stock>(s => s.BookId == bookId && s.Quantity == 0),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

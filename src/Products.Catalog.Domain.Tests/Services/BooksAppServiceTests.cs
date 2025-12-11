using AutoMapper;
using FluentAssertions;
using Moq;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Books;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Enums;
using Products.Catalog.Domain.Interfaces;
using Xunit;

namespace Products.Catalog.Domain.Tests.Services;

public class BooksAppServiceTests
{
    private readonly Mock<IRepository<Book>> _mockBookRepository;
    private readonly Mock<IRepository<Stock>> _mockStockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BooksAppService _service;

    public BooksAppServiceTests()
    {
        _mockBookRepository = new Mock<IRepository<Book>>();
        _mockStockRepository = new Mock<IRepository<Stock>>();
        _mockMapper = new Mock<IMapper>();
        _service = new BooksAppService(_mockBookRepository.Object, _mockStockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
        BookDto? dto = null;
        await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(dto!));
        _mockBookRepository.Verify(r => r.CreateAsync(It.IsAny<Book>()), Times.Never);
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnNull_WhenBookDoesNotExist()
    {
        var bookId = Guid.NewGuid();
        _mockBookRepository.Setup(r => r.ReadAsync(bookId)).ReturnsAsync((Book?)null);

        var result = await _service.ReadAsync(bookId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdate_WhenBookDoesNotExist()
    {
        var bookId = Guid.NewGuid();
        var dto = new BookDto
        {
            Id = bookId,
            Title = "Test Book",
            Author = "Test Author",
            Genre = (int)EBookGenre.Fiction,
            Price = 29.99
        };

        _mockBookRepository.Setup(r => r.ReadAsync(bookId)).ReturnsAsync((Book?)null);

        await _service.UpdateAsync(bookId, dto);

        _mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Book>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteOnlyBook_WhenStockDoesNotExist()
    {
        var bookId = Guid.NewGuid();
        var emptyStocks = new List<Stock>();

        _mockStockRepository.Setup(r => r.FindAsync(string.Empty, 0, int.MaxValue)).ReturnsAsync(emptyStocks);
        _mockBookRepository.Setup(r => r.DeleteAsync(bookId)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(bookId);

        _mockBookRepository.Verify(r => r.DeleteAsync(bookId), Times.Once);
        _mockStockRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task FindAsync_ShouldReturnBookDtos_WhenBooksExist()
    {
        var filterText = "test";
        var books = new List<Book>
        {
            new Book(29.99, "Test Book 1", "Author 1", EBookGenre.Fiction),
            new Book(39.99, "Test Book 2", "Author 2", EBookGenre.NonFiction)
        };

        var dtos = new List<BookDto>
        {
            new BookDto { Id = Guid.NewGuid(), Title = "Test Book 1", Author = "Author 1", Genre = (int)EBookGenre.Fiction, Price = 29.99 },
            new BookDto { Id = Guid.NewGuid(), Title = "Test Book 2", Author = "Author 2", Genre = (int)EBookGenre.NonFiction, Price = 39.99 }
        };

        _mockBookRepository.Setup(r => r.FindAsync(filterText, 0, 100)).ReturnsAsync(books);
        _mockMapper.Setup(m => m.Map<List<BookDto>>(It.IsAny<List<Book>>())).Returns(dtos);

        var result = await _service.FindAsync(filterText);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }
}


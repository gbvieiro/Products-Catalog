using AutoMapper;
using FluentAssertions;
using Moq;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Stocks;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Enums;
using Products.Catalog.Domain.Interfaces;
using Xunit;

namespace Products.Catalog.Domain.Tests.Services;

public class StocksAppServiceTests
{
    private readonly Mock<IRepository<Stock>> _mockStockRepository;
    private readonly Mock<IRepository<Book>> _mockBookRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly StocksAppService _service;

    public StocksAppServiceTests()
    {
        _mockStockRepository = new Mock<IRepository<Stock>>();
        _mockBookRepository = new Mock<IRepository<Book>>();
        _mockMapper = new Mock<IMapper>();
        _service = new StocksAppService(_mockStockRepository.Object, _mockBookRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
        StockDto? dto = null;
        await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(dto!));
        _mockStockRepository.Verify(r => r.CreateAsync(It.IsAny<Stock>()), Times.Never);
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnNull_WhenStockDoesNotExist()
    {
        var stockId = Guid.NewGuid();
        _mockStockRepository.Setup(r => r.ReadAsync(stockId)).ReturnsAsync((Stock?)null);

        var result = await _service.ReadAsync(stockId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteStock()
    {
        var stockId = Guid.NewGuid();
        _mockStockRepository.Setup(r => r.DeleteAsync(stockId)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(stockId);

        _mockStockRepository.Verify(r => r.DeleteAsync(stockId), Times.Once);
    }

    [Fact]
    public async Task GetStockByBookId_ShouldReturnNull_WhenStockDoesNotExist()
    {
        var bookId = Guid.NewGuid();
        var emptyStocks = new List<Stock>();

        _mockStockRepository.Setup(r => r.FindAsync(string.Empty, 0, int.MaxValue)).ReturnsAsync(emptyStocks);

        var result = await _service.GetStockByBookId(bookId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddItemsToStock_ShouldReturnErrorMessage_WhenStockDoesNotExist()
    {
        var bookId = Guid.NewGuid();
        var emptyStocks = new List<Stock>();

        _mockStockRepository.Setup(r => r.FindAsync(string.Empty, 0, int.MaxValue)).ReturnsAsync(emptyStocks);

        var result = await _service.AddItemsToStock(bookId, 5);

        result.Should().Contain($"Stock for book {bookId} could not be found.");
        _mockStockRepository.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Stock>()), Times.Never);
    }

    [Fact]
    public async Task FindAsync_ShouldReturnStockDtos_WhenStocksExist()
    {
        var filterText = "test";
        var stocks = new List<Stock>
        {
            new Stock(10, Guid.NewGuid()),
            new Stock(20, Guid.NewGuid())
        };

        var dtos = new List<StockDto>
        {
            new StockDto { Id = Guid.NewGuid(), BookId = Guid.NewGuid(), Quantity = 10 },
            new StockDto { Id = Guid.NewGuid(), BookId = Guid.NewGuid(), Quantity = 20 }
        };

        _mockStockRepository.Setup(r => r.FindAsync(filterText, 0, 100)).ReturnsAsync(stocks);
        _mockMapper.Setup(m => m.Map<List<StockDto>>(It.IsAny<List<Stock>>())).Returns(dtos);

        var result = await _service.FindAsync(filterText);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }
}


using FluentAssertions;
using Moq;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Application.Features.Orders.Commands.CreateOrder;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.Exceptions;
using ProductsCatalog.Domain.Repositories;
using Xunit;

namespace ProductsCatalog.Application.Tests.Features.Orders;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly Mock<IBookRepository> _bookRepository = new();
    private readonly Mock<IStockRepository> _stockRepository = new();

    private CreateOrderCommandHandler CreateHandler() =>
        new(_orderRepository.Object, _bookRepository.Object, _stockRepository.Object);

    [Fact]
    public async Task Handle_WithEnoughStock_ReservesStockAndCreatesOrder()
    {
        var book = new Book(10, "Clean Architecture", "Robert C. Martin", EBookGenre.NonFiction);
        var stock = new Stock(book.Id, quantity: 5);

        _bookRepository.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);
        _stockRepository.Setup(r => r.GetByBookIdAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(stock);

        var command = new CreateOrderCommand(Guid.NewGuid(), [new CreateOrderItemRequest(book.Id, 2)]);

        var orderId = await CreateHandler().Handle(command, CancellationToken.None);

        orderId.Should().NotBeEmpty();
        stock.Quantity.Should().Be(3);

        _stockRepository.Verify(r => r.Update(stock), Times.Once);
        _orderRepository.Verify(r => r.AddAsync(
            It.Is<Order>(o => o.TotalAmount == 20 && o.Items.Count == 1),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithoutEnoughStock_ThrowsDomainExceptionAndDoesNotCreateOrder()
    {
        var book = new Book(10, "Clean Architecture", "Robert C. Martin", EBookGenre.NonFiction);
        var stock = new Stock(book.Id, quantity: 1);

        _bookRepository.Setup(r => r.GetByIdAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);
        _stockRepository.Setup(r => r.GetByBookIdAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(stock);

        var command = new CreateOrderCommand(Guid.NewGuid(), [new CreateOrderItemRequest(book.Id, 5)]);

        var act = async () => await CreateHandler().Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
        _orderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithUnknownBook_ThrowsNotFoundException()
    {
        var bookId = Guid.NewGuid();
        _bookRepository.Setup(r => r.GetByIdAsync(bookId, It.IsAny<CancellationToken>())).ReturnsAsync((Book?)null);

        var command = new CreateOrderCommand(Guid.NewGuid(), [new CreateOrderItemRequest(bookId, 1)]);

        var act = async () => await CreateHandler().Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}

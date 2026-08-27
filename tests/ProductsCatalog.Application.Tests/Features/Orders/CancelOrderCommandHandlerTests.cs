using FluentAssertions;
using Moq;
using ProductsCatalog.Application.Features.Orders.Commands.CancelOrder;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.Repositories;
using Xunit;

namespace ProductsCatalog.Application.Tests.Features.Orders;

public class CancelOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly Mock<IStockRepository> _stockRepository = new();

    [Fact]
    public async Task Handle_CancelsOrderAndReplenishesStock()
    {
        var bookId = Guid.NewGuid();
        var order = new Order(Guid.NewGuid(), [new OrderItem(bookId, quantity: 3, unitPrice: 10)]);
        var stock = new Stock(bookId, quantity: 2);

        _orderRepository.Setup(r => r.GetByIdAsync(order.Id, It.IsAny<CancellationToken>())).ReturnsAsync(order);
        _stockRepository.Setup(r => r.GetByBookIdAsync(bookId, It.IsAny<CancellationToken>())).ReturnsAsync(stock);

        var handler = new CancelOrderCommandHandler(_orderRepository.Object, _stockRepository.Object);

        var result = await handler.Handle(new CancelOrderCommand(order.Id), CancellationToken.None);

        order.Status.Should().Be(EOrderStatus.Canceled);
        stock.Quantity.Should().Be(5);
        result.Message.Should().Contain(order.Id.ToString());

        _orderRepository.Verify(r => r.Update(order), Times.Once);
        _stockRepository.Verify(r => r.Update(stock), Times.Once);
    }
}

using FluentAssertions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.Events;
using ProductsCatalog.Domain.Exceptions;
using Xunit;

namespace ProductsCatalog.Domain.Tests.Entities;

public class OrderTests
{
    private static List<OrderItem> OneItem() => [new OrderItem(Guid.NewGuid(), quantity: 2, unitPrice: 10)];

    [Fact]
    public void Constructor_WithItems_ComputesTotalAmountAndRaisesOrderCreatedEvent()
    {
        var customerId = Guid.NewGuid();

        var order = new Order(customerId, OneItem());

        order.Status.Should().Be(EOrderStatus.Created);
        order.TotalAmount.Should().Be(20);
        order.DomainEvents.Should().ContainSingle(e => e is OrderCreatedEvent);
    }

    [Fact]
    public void Constructor_WithoutItems_ThrowsDomainException()
    {
        var act = () => new Order(Guid.NewGuid(), []);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Cancel_CreatedOrder_SetsStatusCanceledAndRaisesEvent()
    {
        var order = new Order(Guid.NewGuid(), OneItem());
        order.ClearDomainEvents();

        order.Cancel();

        order.Status.Should().Be(EOrderStatus.Canceled);
        order.DomainEvents.Should().ContainSingle(e => e is OrderCanceledEvent);
    }

    [Fact]
    public void Cancel_AlreadyCanceledOrder_ThrowsDomainException()
    {
        var order = new Order(Guid.NewGuid(), OneItem());
        order.Cancel();

        var act = order.Cancel;

        act.Should().Throw<DomainException>();
    }
}

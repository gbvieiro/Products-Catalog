using FluentAssertions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Exceptions;
using Xunit;

namespace ProductsCatalog.Domain.Tests.Entities;

public class StockTests
{
    [Fact]
    public void Reserve_WithEnoughQuantity_DecreasesQuantity()
    {
        var stock = new Stock(Guid.NewGuid(), quantity: 10);

        stock.Reserve(4);

        stock.Quantity.Should().Be(6);
    }

    [Fact]
    public void Reserve_WithoutEnoughQuantity_ThrowsDomainException()
    {
        var stock = new Stock(Guid.NewGuid(), quantity: 2);

        var act = () => stock.Reserve(5);

        act.Should().Throw<DomainException>().WithMessage("*Not enough stock*");
    }

    [Fact]
    public void Replenish_IncreasesQuantity()
    {
        var stock = new Stock(Guid.NewGuid(), quantity: 2);

        stock.Replenish(3);

        stock.Quantity.Should().Be(5);
    }

    [Fact]
    public void Constructor_WithNegativeQuantity_ThrowsDomainException()
    {
        var act = () => new Stock(Guid.NewGuid(), quantity: -1);

        act.Should().Throw<DomainException>();
    }
}

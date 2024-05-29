using FluentAssertions;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.Validations;
using Xunit;

namespace Products.Catalog.Domain.Tests.Entities
{
    public class OrderUnitTests
    {
        [Fact]
        public void CreateOrder_WithValidParameters_ResultObjectValidState()
        {
            Action action = () =>
            {
                var item = new OrderItem(Guid.NewGuid(), 10, 50);
                
                new Order(
                    Guid.NewGuid(), Guid.NewGuid(), DateTime.Now,
                    OrderStatusEnum.Created, [item], 50
                );
            };

            action.Should().NotThrow<DomainExceptionValidation>();
        }

        [Fact]
        public void CreateOrderItem_InvalidQuantity_DomainException()
        {
            Action action1 = () =>
            {
                var orderItem = new OrderItem(Guid.NewGuid(), 0, 50);
            };

            Action action2 = () =>
            {
                var orderItem = new OrderItem(Guid.NewGuid(), 0, 50);
            };

            action1.Should().Throw<DomainExceptionValidation>();
            action2.Should().Throw<DomainExceptionValidation>();
        }

        [Fact]
        public void CreateOrder_WithNoItems_ResultObjectValidState()
        {
            Action action = () =>
            {
                var order = new Order(
                    Guid.NewGuid(), Guid.NewGuid(), DateTime.Now,
                    OrderStatusEnum.Created, [], 50
                );
            };

            action.Should().Throw<DomainExceptionValidation>();
        }
    }
}
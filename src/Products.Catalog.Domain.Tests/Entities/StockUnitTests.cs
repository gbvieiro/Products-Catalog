using FluentAssertions;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.Validations;
using Xunit;

namespace Products.Catalog.Domain.Tests.Entities
{
    /// <summary>
    /// Stock entity unit tests.
    /// </summary>
    public class StockUnitTests
    {
        /// <summary>
        /// Validate if could create a valid stock.
        /// </summary>
        [Fact]
        public void CreateStock_WithValidParameters_ResultObjectValidState()
        {
            Action action = () =>
            {
                var stock = new Stock(Guid.NewGuid(), 10, Guid.NewGuid());
            };

            action.Should().NotThrow<DomainExceptionValidation>();
        }

        [Fact]
        public void CreateStock_WithInvalidId_ResultObjectValidState()
        {
            Action action = () =>
            {
                var stock = new Stock(Guid.NewGuid(), 10, Guid.NewGuid());
            };

            action.Should().NotThrow<DomainExceptionValidation>();
        }

        /// <summary>
        /// Validate if could create a valid stock.
        /// </summary>
        [Fact]
        public void CreateStock_WithInvalidQuantity_DomainException()
        {
            Action action = () =>
            {
                var stock = new Stock(Guid.NewGuid(), -10, Guid.NewGuid());
            };

            action.Should().Throw<DomainExceptionValidation>();
        }
    }
}
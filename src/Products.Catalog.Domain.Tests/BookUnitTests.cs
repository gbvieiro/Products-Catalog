using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Validations;
using FluentAssertions;
using Xunit;

namespace Products.Catalog.Domain.Tests
{
    public class BookUnitTests
    {
        [Fact]
        public void CreateBook_WithValidParameters_ResultObjectValidState()
        {
            Action action = () =>
            {
                new Book(Guid.NewGuid(), 22.50, 3, "The Shining", "Stephen King", BookGenre.Horror);
            };

            action.Should().NotThrow<DomainExceptionValidation>();
        }

        [Fact]
        public void CreateBook_NegativePrice_DomainException()
        {
            Action action = () => new Book(Guid.NewGuid(), -10, 3, "The Shining", "Stephen King", BookGenre.Horror);
            action.Should().Throw<DomainExceptionValidation>();
        }
    }
}
using FluentAssertions;
using Products.Catalog.Domain.Entities.Users;
using Products.Catalog.Domain.Validations;
using Xunit;

namespace Products.Catalog.Domain.Tests.Entities
{
    /// <summary>
    /// User entity unit tests.
    /// </summary>
    public class UserUnitTests
    {
        [Fact]
        public void CreateUser_WithValidParameters_ResultObjectValidState()
        {
            Action action = () =>
            {
                new User(Guid.NewGuid(), "email@email.com", "Password", "Admin");
            };

            action.Should().NotThrow<DomainExceptionValidation>();
        }

        [Fact]
        public void CreateUser_InvalidEmail_DomainException()
        {
            Action action1 = () =>
            {
                new User(Guid.NewGuid(), "email", "Password", "Admin");
            };

            Action action2 = () =>
            {
                new User(Guid.NewGuid(), string.Empty, "Password", "Admin");
            };

            action1.Should().Throw<DomainExceptionValidation>();
            action2.Should().Throw<DomainExceptionValidation>();
        }

        [Fact]
        public void CreateUser_NoPassword_DomainException()
        {
            Action action = () =>
            {
                new User(Guid.NewGuid(), "email", string.Empty, "Admin");
            };

            action.Should().Throw<DomainExceptionValidation>();
        }

        [Fact]
        public void CreateUser_NoRule_DomainException()
        {
            Action action = () =>
            {
                new User(Guid.NewGuid(), "email", "Password", string.Empty);
            };

            action.Should().Throw<DomainExceptionValidation>();
        }
    }
}
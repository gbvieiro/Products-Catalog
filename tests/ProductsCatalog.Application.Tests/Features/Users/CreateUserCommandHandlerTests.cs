using FluentAssertions;
using Moq;
using ProductsCatalog.Application.Common.Interfaces;
using ProductsCatalog.Application.Features.Users.Commands.CreateUser;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.Repositories;
using Xunit;

namespace ProductsCatalog.Application.Tests.Features.Users;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();

    [Fact]
    public async Task Handle_HashesPasswordBeforeCreatingUser()
    {
        _passwordHasher.Setup(h => h.Hash("s3cret!")).Returns("hashed-value");

        var handler = new CreateUserCommandHandler(_userRepository.Object, _passwordHasher.Object);
        var command = new CreateUserCommand("gabriel@example.com", "s3cret!", ERole.Administrator);

        var userId = await handler.Handle(command, CancellationToken.None);

        userId.Should().NotBeEmpty();

        _userRepository.Verify(r => r.AddAsync(
            It.Is<User>(u => u.PasswordHash == "hashed-value" && u.Email.Address == "gabriel@example.com"),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

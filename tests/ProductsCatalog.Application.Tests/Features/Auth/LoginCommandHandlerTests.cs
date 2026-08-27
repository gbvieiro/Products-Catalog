using FluentAssertions;
using Moq;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Application.Common.Interfaces;
using ProductsCatalog.Application.Features.Auth.Commands.Login;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.ValueObjects;
using Xunit;

namespace ProductsCatalog.Application.Tests.Features.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator = new();

    private LoginCommandHandler CreateHandler() =>
        new(_userRepository.Object, _passwordHasher.Object, _jwtTokenGenerator.Object);

    [Fact]
    public async Task Handle_WithValidCredentials_ReturnsTokenWithUserInfo()
    {
        var user = new User(new Email("admin@email.com"), "hashed-password", ERole.Administrator);

        _userRepository.Setup(r => r.GetByEmailAsync("admin@email.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordHasher.Setup(h => h.Verify("admin", "hashed-password")).Returns(true);

        var expiresAt = DateTime.UtcNow.AddHours(8);
        _jwtTokenGenerator
            .Setup(g => g.Generate(user.Id, "admin@email.com", ERole.Administrator))
            .Returns(new JwtToken("fake-token", expiresAt));

        var handler = CreateHandler();
        var result = await handler.Handle(new LoginCommand("admin@email.com", "admin"), CancellationToken.None);

        result.Token.Should().Be("fake-token");
        result.ExpiresAtUtc.Should().Be(expiresAt);
        result.User.Id.Should().Be(user.Id);
        result.User.Email.Should().Be("admin@email.com");
        result.User.Role.Should().Be(ERole.Administrator);
    }

    [Fact]
    public async Task Handle_WithUnknownEmail_ThrowsUnauthorized()
    {
        _userRepository.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = CreateHandler();
        var act = () => handler.Handle(new LoginCommand("unknown@email.com", "whatever"), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_WithWrongPassword_ThrowsUnauthorized()
    {
        var user = new User(new Email("admin@email.com"), "hashed-password", ERole.Administrator);

        _userRepository.Setup(r => r.GetByEmailAsync("admin@email.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _passwordHasher.Setup(h => h.Verify(It.IsAny<string>(), "hashed-password")).Returns(false);

        var handler = CreateHandler();
        var act = () => handler.Handle(new LoginCommand("admin@email.com", "wrong-password"), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();

        // Nunca deve gerar token quando a senha esta errada.
        _jwtTokenGenerator.Verify(
            g => g.Generate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<ERole>()), Times.Never);
    }
}

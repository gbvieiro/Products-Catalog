using AutoMapper;
using FluentAssertions;
using Moq;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Users;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Interfaces;
using Products.Catalog.Domain.ValueObject;
using Xunit;

namespace Products.Catalog.Domain.Tests.Services;

public class UsersAppServiceTests
{
    private readonly Mock<IRepository<User>> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UsersAppService _service;

    public UsersAppServiceTests()
    {
        _mockRepository = new Mock<IRepository<User>>();
        _mockMapper = new Mock<IMapper>();
        _service = new UsersAppService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenDtoIsValid()
    {
        var dto = new UserDto
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            Password = "password123",
            Role = "Admin"
        };

        var user = new User(new Email(dto.Email), dto.Password, dto.Role);
        _mockMapper.Setup(m => m.Map<User>(dto)).Returns(user);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto);

        result.Should().Be(user.Id);
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
        UserDto? dto = null;

        await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(dto!));
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnUserDto_WhenUserExists()
    {
        var user = new User(new Email("test@example.com"), "password123", "Admin");
        var userId = user.Id;

        var dto = new UserDto
        {
            Id = userId,
            Email = "test@example.com",
            Password = "password123",
            Role = "Admin"
        };

        _mockRepository.Setup(r => r.ReadAsync(userId)).ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(dto);

        var result = await _service.ReadAsync(userId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        _mockRepository.Setup(r => r.ReadAsync(userId)).ReturnsAsync((User?)null);

        var result = await _service.ReadAsync(userId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
    {
        var existingUser = new User(new Email("old@example.com"), "oldpass", "User");
        var userId = existingUser.Id;

        var dto = new UserDto
        {
            Id = userId,
            Email = "new@example.com",
            Password = "newpass",
            Role = "Admin"
        };

        var updatedUser = new User(new Email(dto.Email), dto.Password, dto.Role);

        _mockRepository.Setup(r => r.ReadAsync(userId)).ReturnsAsync(existingUser);
        _mockMapper.Setup(m => m.Map<User>(dto)).Returns(updatedUser);
        _mockRepository.Setup(r => r.UpdateAsync(userId, It.IsAny<User>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(userId, dto);

        _mockRepository.Verify(r => r.UpdateAsync(userId, It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdate_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var dto = new UserDto
        {
            Id = userId,
            Email = "test@example.com",
            Password = "password123",
            Role = "Admin"
        };

        _mockRepository.Setup(r => r.ReadAsync(userId)).ReturnsAsync((User?)null);

        await _service.UpdateAsync(userId, dto);

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser()
    {
        var userId = Guid.NewGuid();
        _mockRepository.Setup(r => r.DeleteAsync(userId)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(userId);

        _mockRepository.Verify(r => r.DeleteAsync(userId), Times.Once);
    }

    [Fact]
    public async Task FindAsync_ShouldReturnUserDtos_WhenUsersExist()
    {
        var filterText = "test";
        var users = new List<User>
        {
            new User(new Email("test1@example.com"), "pass1", "Admin"),
            new User(new Email("test2@example.com"), "pass2", "User")
        };

        var dtos = new List<UserDto>
        {
            new UserDto { Id = Guid.NewGuid(), Email = "test1@example.com", Password = "pass1", Role = "Admin" },
            new UserDto { Id = Guid.NewGuid(), Email = "test2@example.com", Password = "pass2", Role = "User" }
        };

        _mockRepository.Setup(r => r.FindAsync(filterText, 0, 100)).ReturnsAsync(users);
        _mockMapper.Setup(m => m.Map<List<UserDto>>(It.IsAny<List<User>>())).Returns(dtos);

        var result = await _service.FindAsync(filterText);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }
}


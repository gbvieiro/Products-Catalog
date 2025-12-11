using AutoMapper;
using FluentAssertions;
using Moq;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Orders;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Enums;
using Products.Catalog.Domain.Interfaces;
using Xunit;

namespace Products.Catalog.Domain.Tests.Services;

public class OrdersAppServiceTests
{
    private readonly Mock<IRepository<Order>> _mockOrderRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly OrdersAppService _service;

    public OrdersAppServiceTests()
    {
        _mockOrderRepository = new Mock<IRepository<Order>>();
        _mockMapper = new Mock<IMapper>();
        _service = new OrdersAppService(_mockOrderRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
    {
        OrderDto? dto = null;
        await Xunit.Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(dto!));
        _mockOrderRepository.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnNull_WhenOrderDoesNotExist()
    {
        var orderId = Guid.NewGuid();
        _mockOrderRepository.Setup(r => r.ReadAsync(orderId)).ReturnsAsync((Order?)null);

        var result = await _service.ReadAsync(orderId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdate_WhenOrderDoesNotExist()
    {
        var orderId = Guid.NewGuid();
        var dto = new OrderDto
        {
            Id = orderId,
            CustomerId = Guid.NewGuid(),
            CreationData = DateTime.Now,
            Status = (int)EOrderStatus.Created,
            Items = new List<OrderItemDto>(),
            TotalAmount = 0
        };

        _mockOrderRepository.Setup(r => r.ReadAsync(orderId)).ReturnsAsync((Order?)null);

        await _service.UpdateAsync(orderId, dto);

        _mockOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteOrder()
    {
        var orderId = Guid.NewGuid();
        _mockOrderRepository.Setup(r => r.DeleteAsync(orderId)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(orderId);

        _mockOrderRepository.Verify(r => r.DeleteAsync(orderId), Times.Once);
    }

    [Fact]
    public async Task CancelAsync_ShouldReturnErrorMessage_WhenOrderDoesNotExist()
    {
        var orderId = Guid.NewGuid();
        _mockOrderRepository.Setup(r => r.ReadAsync(orderId)).ReturnsAsync((Order?)null);

        var result = await _service.CancelAsync(orderId);

        result.Should().Contain($"Could not found order {orderId}");
    }
}
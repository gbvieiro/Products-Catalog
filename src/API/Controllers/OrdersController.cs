using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Orders;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IOrdersAppService ordersAppService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<Guid> CreateAsync([FromBody] OrderDto orderDto)
    {
        return await ordersAppService.CreateAsync(orderDto);
    }

    [HttpGet("{id}")]
    public async Task<OrderDto?> ReadAsync([FromRoute] Guid id)
    {
        return await ordersAppService.ReadAsync(id);
    }

    [HttpPut("{orderId}/cancel")]
    public async Task<IActionResult> CancelAsync([FromRoute] Guid orderId)
    {
        var responseMessage = await ordersAppService.CancelAsync(orderId);
        return Ok(responseMessage);
    }

    [HttpGet("MyOrders")]
    public async Task<IActionResult> MyOrdersAsync([FromQuery] TextFilterDto filter)
    {
        var booksDtos = await ordersAppService.GetAllAsync(
            filter.Text ?? string.Empty, 0, 100, new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!)
        );

        return Ok(booksDtos);
    }

    [HttpGet()]
    public async Task<IReadOnlyCollection<OrderDto>> FindAsync([FromQuery] TextFilterDto filter)
    {
        return await ordersAppService.FindAsync(filter.Text ?? "");
    }
}
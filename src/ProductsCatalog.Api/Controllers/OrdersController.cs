using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Application.Features.Orders;
using ProductsCatalog.Application.Features.Orders.Commands.CancelOrder;
using ProductsCatalog.Application.Features.Orders.Commands.CreateOrder;
using ProductsCatalog.Application.Features.Orders.Queries.GetOrderById;
using ProductsCatalog.Application.Features.Orders.Queries.GetOrders;

namespace ProductsCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateOrderCommand command)
    {
        var id = await sender.Send(command);
        return CreatedAtAction(nameof(ReadAsync), new { id }, id);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderDto>> ReadAsync(Guid id)
    {
        var order = await sender.Send(new GetOrderByIdQuery(id));
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPut("{id:guid}/cancel")]
    public async Task<ActionResult<CancelOrderResult>> CancelAsync(Guid id)
    {
        var result = await sender.Send(new CancelOrderCommand(id));
        return Ok(result);
    }

    /// <summary>
    /// Pedidos do usuario autenticado. NOTA: este projeto ainda nao configura
    /// um esquema de autenticacao (AddAuthentication/AddJwtBearer) em
    /// Program.cs apesar do pacote JwtBearer estar referenciado - isso ja
    /// vinha assim no projeto original. Sem login real, o claim abaixo nao
    /// existira e o endpoint respondera 400. Ver README/aviso na resposta final.
    /// </summary>
    [HttpGet("my-orders")]
    public async Task<ActionResult<PagedResult<OrderDto>>> MyOrdersAsync(
        [FromQuery] string? filter, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(customerIdClaim, out var customerId))
        {
            return BadRequest("Missing or invalid authenticated user id.");
        }

        var result = await sender.Send(new GetOrdersQuery(customerId, filter, skip, take));
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<OrderDto>>> FindAsync(
        [FromQuery] string? filter, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var result = await sender.Send(new GetOrdersQuery(null, filter, skip, take));
        return Ok(result);
    }
}

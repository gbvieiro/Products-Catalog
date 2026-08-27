using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Application.Features.Orders;
using ProductsCatalog.Application.Features.Orders.Commands.CancelOrder;
using ProductsCatalog.Application.Features.Orders.Commands.CreateOrder;
using ProductsCatalog.Application.Features.Orders.Queries.GetOrderById;
using ProductsCatalog.Application.Features.Orders.Queries.GetOrders;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateOrderCommand command)
    {
        var id = await sender.Send(command);
        return CreatedAtRoute("GetOrderById", new { id }, id);
    }

    [HttpGet("{id:guid}", Name = "GetOrderById")]
    public async Task<ActionResult<OrderDto>> ReadAsync(Guid id)
    {
        var order = await sender.Send(new GetOrderByIdQuery(id));
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPut("{id:guid}/cancel")]
    [Authorize(Roles = nameof(ERole.Administrator))]
    public async Task<ActionResult<CancelOrderResult>> CancelAsync(Guid id)
    {
        var result = await sender.Send(new CancelOrderCommand(id));
        return Ok(result);
    }

    /// <summary>
    /// Pedidos do cliente autenticado. NOTA: o claim NameIdentifier do JWT
    /// carrega o Id do <c>User</c> (usuario/vendedor) logado, nao o Id de um
    /// <c>Customer</c> - desde a introducao da entidade Customer, pedidos sao
    /// vinculados a um Customer, e nao ha relacao entre User e Customer neste
    /// projeto. Por isso este endpoint permanece nao funcional na pratica
    /// (retornara sempre uma lista vazia) - isso e uma limitacao preexistente,
    /// fora do escopo desta alteracao. Considere remover ou redesenhar este
    /// endpoint caso "meus pedidos" volte a ser um requisito real.
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

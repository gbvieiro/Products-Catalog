using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Application.Features.Stocks;
using ProductsCatalog.Application.Features.Stocks.Commands.AddStock;
using ProductsCatalog.Application.Features.Stocks.Commands.CreateStock;
using ProductsCatalog.Application.Features.Stocks.Commands.DeleteStock;
using ProductsCatalog.Application.Features.Stocks.Commands.UpdateStock;
using ProductsCatalog.Application.Features.Stocks.Queries.GetStockByBookId;
using ProductsCatalog.Application.Features.Stocks.Queries.GetStockById;
using ProductsCatalog.Application.Features.Stocks.Queries.GetStocks;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Api.Controllers;

// Leitura (GET) e liberada pra qualquer usuario autenticado - Seller precisa
// ver o estoque disponivel para montar um pedido. Escrita e so pra Administrator.
[ApiController]
[Route("api/[controller]")]
public class StocksController(ISender sender) : ControllerBase
{
    [Authorize(Roles = nameof(ERole.Administrator))]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateStockCommand command)
    {
        var id = await sender.Send(command);
        return CreatedAtRoute("GetStockById", new { id }, id);
    }

    [HttpGet("{id:guid}", Name = "GetStockById")]
    public async Task<ActionResult<StockDto>> ReadAsync(Guid id)
    {
        var stock = await sender.Send(new GetStockByIdQuery(id));
        return stock is null ? NotFound() : Ok(stock);
    }

    [HttpGet("book/{bookId:guid}")]
    public async Task<ActionResult<CompleteStockDto>> GetByBookIdAsync(Guid bookId)
    {
        var stock = await sender.Send(new GetStockByBookIdQuery(bookId));
        return stock is null ? NotFound() : Ok(stock);
    }

    [Authorize(Roles = nameof(ERole.Administrator))]
    [HttpPut("book/{bookId:guid}/add")]
    public async Task<ActionResult<AddStockResult>> AddStockAsync(Guid bookId, [FromBody] AddStockRequest request)
    {
        var result = await sender.Send(new AddStockCommand(bookId, request.Quantity));
        return Ok(result);
    }

    /// <summary>
    /// Ajuste administrativo: define a quantidade em estoque para um valor
    /// absoluto (diferente de AddStockAsync acima, que soma um delta).
    /// </summary>
    [Authorize(Roles = nameof(ERole.Administrator))]
    [HttpPut("book/{bookId:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid bookId, [FromBody] UpdateStockRequest request)
    {
        await sender.Send(new UpdateStockCommand(bookId, request.Quantity));
        return NoContent();
    }

    [Authorize(Roles = nameof(ERole.Administrator))]
    [HttpDelete("book/{bookId:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid bookId)
    {
        await sender.Send(new DeleteStockCommand(bookId));
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<StockDto>>> FindAsync(
        [FromQuery] string? filter, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var result = await sender.Send(new GetStocksQuery(filter, skip, take));
        return Ok(result);
    }

    public sealed record AddStockRequest(int Quantity);

    public sealed record UpdateStockRequest(int Quantity);
}

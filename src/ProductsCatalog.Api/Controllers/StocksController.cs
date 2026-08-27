using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Application.Features.Stocks;
using ProductsCatalog.Application.Features.Stocks.Commands.AddStock;
using ProductsCatalog.Application.Features.Stocks.Commands.CreateStock;
using ProductsCatalog.Application.Features.Stocks.Queries.GetStockByBookId;
using ProductsCatalog.Application.Features.Stocks.Queries.GetStockById;
using ProductsCatalog.Application.Features.Stocks.Queries.GetStocks;

namespace ProductsCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateStockCommand command)
    {
        var id = await sender.Send(command);
        return CreatedAtAction(nameof(ReadAsync), new { id }, id);
    }

    [HttpGet("{id:guid}")]
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

    [HttpPut("book/{bookId:guid}/add")]
    public async Task<ActionResult<AddStockResult>> AddStockAsync(Guid bookId, [FromBody] AddStockRequest request)
    {
        var result = await sender.Send(new AddStockCommand(bookId, request.Quantity));
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<StockDto>>> FindAsync(
        [FromQuery] string? filter, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var result = await sender.Send(new GetStocksQuery(filter, skip, take));
        return Ok(result);
    }

    public sealed record AddStockRequest(int Quantity);
}

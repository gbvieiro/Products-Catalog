using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Stocks;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StocksController(IStocksAppService stocksAppService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<StockDto?> ReadAsync([FromRoute] Guid id)
    {
        return await stocksAppService.ReadAsync(id);
    }

    [HttpGet("book/{bookId}")]
    public async Task<CompleteStockDto?> GetByBookIdAsync([FromRoute] Guid bookId)
    {
        return await stocksAppService.GetStockByBookId(bookId);
    }

    [HttpPut("book/{bookId}/AddBooks")]
    public async Task<string> AddBooksAsync([FromRoute] Guid bookId, [FromBody] AddStockDto dto)
    {
        return await stocksAppService.AddItemsToStock(bookId, dto.Quantity);
    }

    [HttpPost("create")]
    public async Task<Guid> CreateAsync([FromBody] StockDto stockDto)
    {
        return await stocksAppService.CreateAsync(stockDto);
    }

    [HttpGet()]
    public async Task<IReadOnlyCollection<StockDto>> FindAsync([FromQuery] TextFilterDto filter)
    {
        return await stocksAppService.FindAsync(filter.Text ?? "");
    }
}
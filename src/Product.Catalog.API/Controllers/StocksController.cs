using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.DTOs.Stocks;
using Products.Catalog.Application.Services.Stocks;
using Products.Catalog.Infra.Authentication;

namespace Product.Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController(IStocksAppService stocksAppService) : ControllerBase
    {
        private readonly IStocksAppService _stocksAppService = stocksAppService;

        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin},{AuthenticationConfigs.Seller}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            var stockDto = await _stocksAppService.GetAsync(id);
            return Ok(stockDto);
        }

        [HttpGet("book/{bookId}")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin},{AuthenticationConfigs.Seller}")]
        public async Task<IActionResult> GetByBookIdAsync([FromRoute] Guid bookId)
        {
            var stockDto = await _stocksAppService.GetStockByBookId(bookId);
            return Ok(stockDto);
        }

        [HttpPut("book/{bookId}/AddBooks")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin}")]
        public async Task<IActionResult> AddBooksAsync([FromRoute] Guid bookId, [FromBody] AddStockDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            var responseMessage = await _stocksAppService.AddItemsToStock(bookId, dto.Quantity);
            return Ok(responseMessage);
        }

        [HttpPost("Save")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin}")]
        public async Task<IActionResult> SaveAsync([FromBody] StockDto stockDto)
        {
            if (stockDto == null)
            {
                return BadRequest();
            }

            await _stocksAppService.SaveAsync(stockDto);

            return Ok(stockDto.Id);
        }

        [HttpGet()]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin},{AuthenticationConfigs.Seller}")]
        public async Task<IActionResult> GetAllAsync([FromQuery] TextFilterPaginationDTO filter)
        {
            if (filter == null)
            {
                return BadRequest();
            }

            var dtos = await _stocksAppService.GetAllAsync(
                filter.Text ?? string.Empty, filter.Skip, filter.Take
            );

            return Ok(dtos);
        }
    }
}
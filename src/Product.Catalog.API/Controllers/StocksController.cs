using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.DTOs.Stocks;
using Products.Catalog.Application.Services.Stocks;

namespace Product.Catalog.API.Controllers
{
    /// <summary>
    /// Define API methods for Stock.
    /// </summary>
    /// <param name="stocksAppService">A stocks app service instance.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController(IStocksAppService stocksAppService) : ControllerBase
    {
        /// <summary>
        /// A book app service interface.
        /// </summary>
        private readonly IStocksAppService _stocksAppService = stocksAppService;

        /// <summary>
        /// Get stock.
        /// </summary>
        /// <param name="id">A stock id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            var stockDto = await _stocksAppService.GetAsync(id);
            return Ok(stockDto);
        }

        /// <summary>
        /// Get stock.
        /// </summary>
        /// <param name="bookId">A book id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetByBookIdAsync([FromRoute] Guid bookId)
        {
            var stockDto = await _stocksAppService.GetStockByBookId(bookId);
            return Ok(stockDto);
        }

        /// <summary>
        /// Get stock.
        /// </summary>
        /// <param name="bookId">A book id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpPut("book/{bookId}/AddBooks")]
        public async Task<IActionResult> AddBooksAsync([FromRoute] Guid bookId, [FromBody] int quantity)
        {
            var responseMessage = await _stocksAppService.AddItemsToStock(bookId, quantity);
            return Ok(responseMessage);
        }

        /// <summary>
        /// Delete stock.
        /// </summary>
        /// <param name="id">A stock id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _stocksAppService.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Save stock.
        /// </summary>
        /// <param name="stockDto">A stock dto.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] StockDto stockDto)
        {
            if (stockDto == null)
            {
                return BadRequest();
            }

            await _stocksAppService.SaveAsync(stockDto);

            return Ok(stockDto.Id);
        }

        /// <summary>
        /// Get all stocks.
        /// </summary>
        /// <param name="filter">Filter parameters.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet()]
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
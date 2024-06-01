using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.Services.Orders;

namespace Product.Catalog.API.Controllers
{
    /// <summary>
    /// Define API methods for orders.
    /// </summary>
    /// <param name="ordersAppService">A orders app service instance.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrdersAppService ordersAppService) : ControllerBase
    {
        /// <summary>
        /// A orders app service interface.
        /// </summary>
        private readonly IOrdersAppService _ordersAppService = ordersAppService;

        /// <summary>
        /// Get order.
        /// </summary>
        /// <param name="id">A order id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            var dto = await _ordersAppService.GetAsync(id);
            return Ok(dto);
        }

        /// <summary>
        /// Delete order.
        /// </summary>
        /// <param name="id">A order id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _ordersAppService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Save order.
        /// </summary>
        /// <param name="orderDto">A order dto.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest();
            }

            await _ordersAppService.SaveAsync(orderDto);

            return Ok(orderDto.Id);
        }

        /// <summary>
        /// Get all orders.
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

            var booksDtos = await _ordersAppService.GetAllAsync(
                filter.Text ?? string.Empty, filter.Skip, filter.Take
            );

            return Ok(booksDtos);
        }
    }
}
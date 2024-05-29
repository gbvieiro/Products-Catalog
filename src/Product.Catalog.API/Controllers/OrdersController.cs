using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.DTOs;

namespace Product.Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        /// <summary>
        /// Get book.
        /// </summary>
        /// <param name="id">A book id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            return Ok(id);
        }

        /// <summary>
        /// Cancel order.
        /// </summary>
        /// <param name="id">A order id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelAsync([FromRoute] string id)
        {
            return Ok(id);
        }

        /// <summary>
        /// Save order.
        /// </summary>
        /// <param name="brandsIds">A list of brands ids.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] OrderDto bookDTO)
        {
            return Ok(bookDTO);
        }

        /// <summary>
        /// Get all order.
        /// </summary>
        /// <param name="filter">Filter parameters.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllAsync([FromQuery] TextFilterPaginationDTO filter)
        {
            return Ok(filter);
        }

        /// <summary>
        /// Get all order.
        /// </summary>
        /// <param name="filter">Filter parameters.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet("My")]
        public async Task<IActionResult> GetMyOrdersAsync([FromQuery] TextFilterPaginationDTO filter)
        {
            return Ok(filter);
        }
    }
}
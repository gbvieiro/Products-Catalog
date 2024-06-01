using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.Services.Orders;
using Products.Catalog.Infra.Authentication;
using System.Security.Claims;

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
        /// <param name="orderId">A order id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin},{AuthenticationConfigs.Seller}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid orderId)
        {
            var dto = await _ordersAppService.GetAsync(orderId);
            return Ok(dto);
        }

        /// <summary>
        /// Cancel order.
        /// </summary>
        /// <param name="orderId">orderId</param>
        /// <returns></returns>
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin},{AuthenticationConfigs.Seller}")]
        public async Task<IActionResult> CancelAsync([FromRoute] Guid orderId)
        {
            var responseMessage = await _ordersAppService.CancelAsync(orderId);
            return Ok(responseMessage);
        }

        /// <summary>
        /// Save order.
        /// </summary>
        /// <param name="orderDto">A order dto.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpPost("Save")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin},{AuthenticationConfigs.Seller}")]
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
        /// Get all orders for the current user.
        /// </summary>
        /// <param name="filter">Filter parameters.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet("MyOrders")]
        [Authorize]
        public async Task<IActionResult> MyOrdersAsync([FromQuery] TextFilterPaginationDTO filter)
        {
            if (filter == null)
            {
                return BadRequest();
            }

            var booksDtos = await _ordersAppService.GetAllAsync(
                filter.Text ?? string.Empty, filter.Skip, filter.Take, new Guid(this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!)
            );

            return Ok(booksDtos);
        }

        /// <summary>
        /// Get all orders.
        /// </summary>
        /// <param name="filter">Filter parameters.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet()]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin},{AuthenticationConfigs.Seller}")]
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
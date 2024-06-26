using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.Services.Orders;
using Products.Catalog.Infra.Authentication;
using System.Security.Claims;

namespace Product.Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrdersAppService ordersAppService) : ControllerBase
    {
        private readonly IOrdersAppService _ordersAppService = ordersAppService;

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid orderId)
        {
            var dto = await _ordersAppService.GetAsync(orderId);
            return Ok(dto);
        }

        [HttpPut("{orderId}/cancel")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin},{AuthenticationConfigs.Seller}")]
        public async Task<IActionResult> CancelAsync([FromRoute] Guid orderId)
        {
            var responseMessage = await _ordersAppService.CancelAsync(orderId);
            return Ok(responseMessage);
        }

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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.Services.Users;
using Products.Catalog.Infra.Authentication;

namespace Product.Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUsersAppService usersAppService) : ControllerBase
    {
        private readonly IUsersAppService _usersAppService = usersAppService;

        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin},{AuthenticationConfigs.Seller}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            var dto = await _usersAppService.GetAsync(id);
            return Ok(dto);
        }

        [HttpPost("Save")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveAsync([FromBody] UserDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            await _usersAppService.SaveAsync(dto);

            return Ok(dto.Id);
        }

        [HttpGet()]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin},{AuthenticationConfigs.Seller}")]
        public async Task<IActionResult> GetAllAsync([FromQuery] TextFilterPaginationDTO filter)
        {
            if (filter == null)
            {
                return BadRequest();
            }

            var dtos = await _usersAppService.GetAllAsync(filter.Text ?? string.Empty, filter.Skip, filter.Take);

            return Ok(dtos);
        }
    }
}
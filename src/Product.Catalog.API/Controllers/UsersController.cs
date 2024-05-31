﻿using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.Services.Users;

namespace Product.Catalog.API.Controllers
{
    /// <summary>
    /// Define API methods for User.
    /// </summary>
    /// <param name="usersAppService">A users app service instance.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUsersAppService usersAppService) : ControllerBase
    {
        /// <summary>
        /// A book app service interface.
        /// </summary>
        private readonly IUsersAppService _usersAppService = usersAppService;

        /// <summary>
        /// Get user.
        /// </summary>
        /// <param name="id">A user id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            var dto = await _usersAppService.GetAsync(id);
            return Ok(dto);
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">A user id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _usersAppService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Save user.
        /// </summary>
        /// <param name="stockDto">A stock dto.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] UserDto dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            await _usersAppService.SaveAsync(dto);

            return Ok(dto.Id);
        }

        /// <summary>
        /// Get all users.
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

            var dtos = await _usersAppService.GetAllAsync(
                filter.Text ?? string.Empty, filter.Skip, filter.Take
            );

            return Ok(dtos);
        }
    }
}
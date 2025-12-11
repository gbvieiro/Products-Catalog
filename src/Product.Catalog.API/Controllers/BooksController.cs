using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.Services.Books;
using Products.Catalog.Infra.Authentication;

namespace Product.Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(IBooksAppService booksAppSerice) : ControllerBase
    {
        private readonly IBooksAppService _booksAppSerice = booksAppSerice;

        [HttpPost("Create")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateAsync([FromBody] BookDto bookDTO)
        {
            if (bookDTO == null)
                return BadRequest();

            await _booksAppSerice.SaveAsync(bookDTO);

            return Ok(bookDTO);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin}")]
        public async Task<IActionResult> ReadAsync([FromRoute] Guid id)
        {
            var dto = await _booksAppSerice.GetAsync(id);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] BookDto bookDTO)
        {
            if (bookDTO == null)
                return BadRequest();

            await _booksAppSerice.SaveAsync(bookDTO);

            return Ok(bookDTO);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _booksAppSerice.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet()]
        [Authorize(Roles = $"{AuthenticationConfigs.Admin}")]
        public async Task<IActionResult> FindAsync([FromQuery] TextFilterPaginationDTO filter)
        {
            if (filter == null)
                return BadRequest();

            var booksDtos = await _booksAppSerice.GetAllAsync(
                filter.Text ?? string.Empty, filter.Skip, filter.Take
            );

            return Ok(booksDtos);
        }
    }
}
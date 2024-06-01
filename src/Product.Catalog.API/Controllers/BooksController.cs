using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.Services.Books;

namespace Product.Catalog.API.Controllers
{
    /// <summary>
    /// Define API methods for Book.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(IBooksAppService booksAppSerice) : ControllerBase
    {
        /// <summary>
        /// A book app service interface.
        /// </summary>
        private readonly IBooksAppService _booksAppSerice = booksAppSerice;

        /// <summary>
        /// Get book informing the ID.
        /// </summary>
        /// <param name="id">A book ID.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            var dto = await _booksAppSerice.GetAsync(id);
            return Ok(dto);
        }

        /// <summary>
        /// Delete book.
        /// </summary>
        /// <param name="id">A book id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _booksAppSerice.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Save book.
        /// </summary>
        /// <param name="brandsIds">A list of brands ids.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpPost("Save")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> SaveAsync([FromBody] BookDto bookDTO)
        {
            if (bookDTO == null)
            {
                return BadRequest();
            }

            await _booksAppSerice.SaveAsync(bookDTO);

            return Ok(bookDTO.Id);
        }

        /// <summary>
        /// Get all books.
        /// </summary>
        /// <param name="filter">Filter parameters.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet()]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllAsync([FromQuery] TextFilterPaginationDTO filter)
        {
            if (filter == null)
            {
                return BadRequest();
            }

            var booksDtos = await _booksAppSerice.GetAllAsync(
                filter.Text ?? string.Empty, filter.Skip, filter.Take
            );

            return Ok(booksDtos);
        }
    }
}
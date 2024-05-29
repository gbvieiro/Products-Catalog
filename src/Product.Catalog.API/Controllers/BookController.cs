using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Filters;
using Products.Catalog.Application.Services.Books;

namespace Product.Catalog.API.Controllers
{
    /// <summary>
    /// Define API methods for BOOK
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        /// <summary>
        /// A book app service interface.
        /// </summary>
        private readonly IBookAppService _bookAppSerice;

        public BookController(IBookAppService bookAppSerice)
        {
            _bookAppSerice = bookAppSerice;
        }

        /// <summary>
        /// Get book.
        /// </summary>
        /// <param name="id">A book id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            var bookDto = await _bookAppSerice.GetAsync(id);
            return Ok(bookDto);
        }

        /// <summary>
        /// Delete book.
        /// </summary>
        /// <param name="id">A book id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _bookAppSerice.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Save book.
        /// </summary>
        /// <param name="brandsIds">A list of brands ids.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] BookDto bookDTO)
        {
            if (bookDTO == null)
            {
                return BadRequest();
            }

            await _bookAppSerice.SaveAsync(bookDTO);

            return Ok(bookDTO.Id);
        }

        /// <summary>
        /// Get all books.
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

            var booksDtos = await _bookAppSerice.GetAllAsync(
                filter.Text ?? string.Empty, filter.Skip, filter.Take
            );

            return Ok(booksDtos);
        }
    }
}
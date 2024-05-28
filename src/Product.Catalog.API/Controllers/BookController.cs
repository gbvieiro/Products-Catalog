using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Filters;

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
        /// Delete book.
        /// </summary>
        /// <param name="id">A book id.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string id)
        {
            return Ok(id);
        }

        /// <summary>
        /// Save book.
        /// </summary>
        /// <param name="brandsIds">A list of brands ids.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] BookDTO bookDTO)
        {
            return Ok(bookDTO);
        }

        /// <summary>
        /// Get all books.
        /// </summary>
        /// <param name="filter">Filter parameters.</param>
        /// <returns>A http response with the status code.</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAllAsync([FromQuery] TextFilterPaginationDTO filter)
        {
            return Ok(filter);
        }
    }
}
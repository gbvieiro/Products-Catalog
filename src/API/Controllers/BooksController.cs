using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Books;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IBooksAppService booksAppSerice) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<Guid> CreateAsync([FromBody] BookDto bookDTO)
    {
        return await booksAppSerice.CreateAsync(bookDTO);
    }

    [HttpGet("{id}")]
    public async Task<BookDto?> ReadAsync([FromRoute] Guid id)
    {
        return await booksAppSerice.ReadAsync(id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] BookDto bookDTO)
    {
        await booksAppSerice.UpdateAsync(id, bookDTO);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await booksAppSerice.DeleteAsync(id);
        return Ok();
    }

    [HttpGet()]
    public async Task<IReadOnlyCollection<BookDto>> FindAsync([FromQuery] TextFilterDto filter)
    {
        return await booksAppSerice.FindAsync(filter.Text ?? string.Empty);
    }
}
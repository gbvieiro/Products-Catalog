using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Application.Features.Books;
using ProductsCatalog.Application.Features.Books.Commands.CreateBook;
using ProductsCatalog.Application.Features.Books.Commands.DeleteBook;
using ProductsCatalog.Application.Features.Books.Commands.UpdateBook;
using ProductsCatalog.Application.Features.Books.Queries.GetBookById;
using ProductsCatalog.Application.Features.Books.Queries.GetBooks;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(ISender sender) : ControllerBase
{
    public sealed record UpdateBookRequest(double Price, string Title, string Author, EBookGenre Genre);

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateBookCommand command)
    {
        var id = await sender.Send(command);
        return CreatedAtRoute("GetBookById", new { id }, id);
    }

    [HttpGet("{id:guid}", Name = "GetBookById")]
    public async Task<ActionResult<BookDto>> ReadAsync(Guid id)
    {
        var book = await sender.Send(new GetBookByIdQuery(id));
        return book is null ? NotFound() : Ok(book);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateBookRequest request)
    {
        await sender.Send(new UpdateBookCommand(id, request.Price, request.Title, request.Author, request.Genre));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await sender.Send(new DeleteBookCommand(id));
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<BookDto>>> FindAsync(
        [FromQuery] string? filter, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var result = await sender.Send(new GetBooksQuery(filter, skip, take));
        return Ok(result);
    }
}

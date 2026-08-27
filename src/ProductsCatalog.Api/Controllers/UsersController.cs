using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Application.Features.Users;
using ProductsCatalog.Application.Features.Users.Commands.CreateUser;
using ProductsCatalog.Application.Features.Users.Queries.GetUserById;
using ProductsCatalog.Application.Features.Users.Queries.GetUsers;

namespace ProductsCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateUserCommand command)
    {
        var id = await sender.Send(command);
        return CreatedAtAction(nameof(ReadAsync), new { id }, id);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> ReadAsync(Guid id)
    {
        var user = await sender.Send(new GetUserByIdQuery(id));
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<UserDto>>> FindAsync(
        [FromQuery] string? filter, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var result = await sender.Send(new GetUsersQuery(filter, skip, take));
        return Ok(result);
    }
}

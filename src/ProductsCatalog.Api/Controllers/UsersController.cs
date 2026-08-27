using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Application.Features.Users;
using ProductsCatalog.Application.Features.Users.Commands.CreateUser;
using ProductsCatalog.Application.Features.Users.Commands.DeleteUser;
using ProductsCatalog.Application.Features.Users.Commands.UpdateUser;
using ProductsCatalog.Application.Features.Users.Queries.GetUserById;
using ProductsCatalog.Application.Features.Users.Queries.GetUsers;
using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Api.Controllers;

/// <summary>Gerenciar contas de usuario (login) e restrito a Administrator - Seller nao tem acesso a nada aqui.</summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(ERole.Administrator))]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateUserCommand command)
    {
        var id = await sender.Send(command);
        return CreatedAtRoute("GetUserById", new { id }, id);
    }

    [HttpGet("{id:guid}", Name = "GetUserById")]
    public async Task<ActionResult<UserDto>> ReadAsync(Guid id)
    {
        var user = await sender.Send(new GetUserByIdQuery(id));
        return user is null ? NotFound() : Ok(user);
    }

    public sealed record UpdateUserRequest(string Email, ERole Role);

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateUserRequest request)
    {
        await sender.Send(new UpdateUserCommand(id, request.Email, request.Role));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await sender.Send(new DeleteUserCommand(id));
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<UserDto>>> FindAsync(
        [FromQuery] string? filter, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var result = await sender.Send(new GetUsersQuery(filter, skip, take));
        return Ok(result);
    }
}

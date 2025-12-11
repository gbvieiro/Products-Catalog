using Microsoft.AspNetCore.Mvc;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.Services.Users;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUsersAppService usersAppService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<UserDto?> ReadAsync([FromRoute] Guid id)
    {
        return await usersAppService.ReadAsync(id);
    }

    [HttpPost("create")]
    public async Task<Guid> CreateAsync([FromBody] UserDto dto)
    {
        return await usersAppService.CreateAsync(dto);
    }

    [HttpGet()]
    public async Task<IReadOnlyCollection<UserDto>> FindAsync([FromQuery] TextFilterDto filter)
    {
        return await usersAppService.FindAsync(filter.Text ?? string.Empty);
    }
}
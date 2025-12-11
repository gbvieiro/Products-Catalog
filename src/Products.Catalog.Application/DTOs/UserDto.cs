using Application.DTOs;

namespace Products.Catalog.Application.DTOs;

public class UserDto : EntityDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}
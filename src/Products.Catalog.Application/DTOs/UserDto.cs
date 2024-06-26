using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.DTOs
{
    public class UserDto : EntityDTO
    {
        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string Role { get; set; }
    }
}
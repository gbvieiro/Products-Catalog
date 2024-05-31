using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.DTOs
{
    /// <summary>
    /// Represents a User structure.
    /// </summary>
    public class UserDto : EntityDTO
    {
        /// <summary>
        /// User email used for login.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// User role name.
        /// </summary>
        public required string Role { get; set; }
    }
}
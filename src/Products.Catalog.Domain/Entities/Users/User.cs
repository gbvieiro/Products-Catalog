using Products.Catalog.Domain.Entities.Base;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Users
{
    /// <summary>
    /// Represents a user in the product catalog.
    /// </summary>
    public class User : IEntity<Guid>
    {
        /// <summary>
        /// A unique identificator.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// User email used for login.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// User password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// User role name.
        /// </summary>
        public string Role { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Id">A unique identificator.</param>
        /// <param name="email">User email used for login.</param>
        /// <param name="password">User password.</param>
        /// <param name="role">User role name.</param>
        public User(Guid id, string email, string password, string role)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = role;

            ValidateDomain();
        }

        /// <summary>
        /// Define rules for a valid book.
        /// </summary>
        private void ValidateDomain()
        {
            // Id
            DomainExceptionValidation.When(Id == Guid.Empty, "ID is required");

            // Email
            DomainExceptionValidation.When(string.IsNullOrEmpty(Email), "Email is required");
            DomainExceptionValidation.When(!EmailValidator.IsValidEmail(Email), "Invalid email format.");

            // Password
            DomainExceptionValidation.When(string.IsNullOrEmpty(Password), "Password is required.");

            //Role
            DomainExceptionValidation.When(string.IsNullOrEmpty(Role), "Role is required.");
        }

        /// <summary>
        /// All entities must be able to verify equality. 
        /// Entities must be unique items.
        /// </summary>
        /// <param name="other">Other entity to compare.</param>
        /// <returns>True if objects are the same entity.</returns>
        public bool Equals(IEntity<Guid>? other)
        {
            return other is User && other.Id == Id;
        }
    }
}
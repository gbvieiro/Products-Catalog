using Products.Catalog.Domain.Entities.Base;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Users
{
    public class User : IEntity<Guid>
    {
        public Guid Id { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public string Role { get; private set; }

        public User(Guid id, string email, string password, string role)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = role;

            ValidateDomain();
        }

        private void ValidateDomain()
        {
            DomainExceptionValidation.When(Id == Guid.Empty, "ID is required");

            DomainExceptionValidation.When(string.IsNullOrEmpty(Email), "Email is required");
            DomainExceptionValidation.When(!EmailValidator.IsValidEmail(Email), "Invalid email format.");

            DomainExceptionValidation.When(string.IsNullOrEmpty(Password), "Password is required.");

            DomainExceptionValidation.When(string.IsNullOrEmpty(Role), "Role is required.");
        }

        public bool Equals(IEntity<Guid>? other)
        {
            return other is User && other.Id == Id;
        }
    }
}
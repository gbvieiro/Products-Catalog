using Products.Catalog.Domain.Validations;
using Products.Catalog.Domain.ValueObject;
using Shared.Entities;

namespace Products.Catalog.Domain.Entities;

public class User : Entity
{
    public Email Email { get; private set; }
    public string Password { get; private set; }
    public string Role { get; private set; }

    protected User()
    {
        Email = new Email(string.Empty);
        Password = string.Empty;
        Role = string.Empty;
    }

    public User(Email email, string password, string role)
    {
        Email = email;
        Password = password;
        Role = role;

        ValidateDomain();
    }

    private void ValidateDomain()
    {
        DomainException.When(Id == Guid.Empty, "ID is required");
        DomainException.When(!Email.IsValid(), "Invalid email format.");
        DomainException.When(string.IsNullOrEmpty(Password), "Password is required.");
        DomainException.When(string.IsNullOrEmpty(Role), "Role is required.");
    }
}
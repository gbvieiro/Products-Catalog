using ProductsCatalog.Domain.Common;
using ProductsCatalog.Domain.Exceptions;
using ProductsCatalog.Domain.ValueObjects;

namespace ProductsCatalog.Domain.Entities;

public class User : BaseEntity
{
    public Email Email { get; private set; } = new();

    /// <summary>Hash da senha. O hashing em si e responsabilidade da Application (porta IPasswordHasher).</summary>
    public string PasswordHash { get; private set; } = string.Empty;

    public string Role { get; private set; } = string.Empty;

    protected User()
    {
    }

    public User(Email email, string passwordHash, string role)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;

        Validate();
    }

    private void Validate()
    {
        DomainException.When(!Email.IsValid(), "Invalid email format.");
        DomainException.When(string.IsNullOrEmpty(PasswordHash), "Password is required.");
        DomainException.When(string.IsNullOrEmpty(Role), "Role is required.");
    }
}

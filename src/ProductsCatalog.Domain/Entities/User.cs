using ProductsCatalog.Domain.Common;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.Exceptions;
using ProductsCatalog.Domain.ValueObjects;

namespace ProductsCatalog.Domain.Entities;

public class User : BaseEntity
{
    public Email Email { get; private set; } = new();

    /// <summary>Hash da senha. O hashing em si e responsabilidade da Application (porta IPasswordHasher).</summary>
    public string PasswordHash { get; private set; } = string.Empty;

    public ERole Role { get; private set; }

    protected User()
    {
    }

    public User(Email email, string passwordHash, ERole role)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;

        Validate();
    }

    /// <summary>
    /// Atualiza os dados editaveis do usuario. A senha fica de fora de proposito
    /// (troca de senha e um fluxo a parte, tipicamente com a senha atual exigida
    /// como confirmacao - fora do escopo desta refatoracao).
    /// </summary>
    public void Update(Email email, ERole role)
    {
        Email = email;
        Role = role;

        Validate();
        Touch();
    }

    private void Validate()
    {
        DomainException.When(!Email.IsValid(), "Invalid email format.");
        DomainException.When(string.IsNullOrEmpty(PasswordHash), "Password is required.");
        DomainException.When(!Enum.IsDefined(Role), "Invalid role.");
    }
}

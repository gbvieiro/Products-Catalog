using ProductsCatalog.Domain.Common;
using ProductsCatalog.Domain.Exceptions;
using ProductsCatalog.Domain.ValueObjects;

namespace ProductsCatalog.Domain.Entities;

/// <summary>
/// Cliente para quem um pedido e feito. E um conceito separado de <see cref="User"/>
/// de proposito: User representa uma conta com login no sistema (email/senha/role),
/// enquanto Customer e apenas quem recebe/paga o pedido - o checkout deste projeto
/// nao exige autenticacao, entao Customer nao precisa de senha nem role.
/// </summary>
public class Customer : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = new();

    protected Customer()
    {
    }

    public Customer(string name, Email email)
    {
        Name = name;
        Email = email;

        Validate();
    }

    public void Update(string name, Email email)
    {
        Name = name;
        Email = email;

        Validate();
        Touch();
    }

    private void Validate()
    {
        DomainException.When(string.IsNullOrEmpty(Name), "Name is required.");
        DomainException.When(Name.Length > 100, "Invalid name, too long, maximum 100 characters.");
        DomainException.When(!Email.IsValid(), "Invalid email format.");
    }
}

using System.Text.RegularExpressions;

namespace ProductsCatalog.Domain.ValueObjects;

/// <summary>Value Object imutavel para enderecos de e-mail.</summary>
public sealed partial class Email(string address)
{
    public Email() : this(string.Empty)
    {
    }

    public string Address { get; private set; } = address;

    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Address))
        {
            return false;
        }

        return EmailRegex().IsMatch(Address);
    }

    public override string ToString() => Address;

    public override bool Equals(object? obj) =>
        obj is Email other && string.Equals(Address, other.Address, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Address.ToLowerInvariant().GetHashCode();

    [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
    private static partial Regex EmailRegex();
}

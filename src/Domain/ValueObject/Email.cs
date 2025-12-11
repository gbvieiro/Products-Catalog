using System.Text.RegularExpressions;

namespace Products.Catalog.Domain.ValueObject;

public class Email(string address)
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

        var pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        var regex = new Regex(pattern);

        return regex.IsMatch(Address);
    }
}
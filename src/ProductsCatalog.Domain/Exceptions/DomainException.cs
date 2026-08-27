namespace ProductsCatalog.Domain.Exceptions;

/// <summary>Violacao de uma regra de negocio do dominio.</summary>
public class DomainException(string errorMessage) : Exception(errorMessage)
{
    public static void When(bool hasError, string errorMessage)
    {
        if (hasError)
        {
            throw new DomainException(errorMessage);
        }
    }
}

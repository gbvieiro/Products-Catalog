namespace Products.Catalog.Domain.Validations
{
    public class DomainExceptionValidation(string errorMessage) : Exception(errorMessage)
    {
        public static void When(bool hasError, string errorMessage)
        {
            if (hasError) { 
                throw new DomainExceptionValidation(errorMessage);
            }
        }
    }
}
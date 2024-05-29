namespace Products.Catalog.Domain.Validations
{
    /// <summary>
    /// A special exception used to identify Domain Exceptions.
    /// When a problem is detected on the aplication domain
    /// this exception is throw to inform user.
    /// </summary>
    /// <param name="errorMessage">An error message.</param>
    public class DomainExceptionValidation(string errorMessage) : Exception(errorMessage)
    {
        /// <summary>
        /// When a condition is confirmed, will throw a DomainExceptionValidation.
        /// </summary>
        /// <param name="hasError">A condition that identifies a error.</param>
        /// <param name="errorMessage">A error message.</param>
        /// <exception cref="DomainExceptionValidation">When a error is confirmed.</exception>
        public static void When(bool hasError, string errorMessage)
        {
            if (hasError) { 
                throw new DomainExceptionValidation(errorMessage);
            }
        }
    }
}
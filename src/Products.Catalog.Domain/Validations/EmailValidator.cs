using System.Text.RegularExpressions;

namespace Products.Catalog.Domain.Validations
{
    /// <summary>
    /// This class provide validation methods for Emails.
    /// </summary>
    public class EmailValidator
    {
        /// <summary>
        /// The informed email is valid?
        /// </summary>
        /// <param name="email">A email.</param>
        /// <returns>True if valid. False if not valid.</returns>
        public static bool IsValidEmail(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            var regex = new Regex(pattern);
            
            return regex.IsMatch(email);
        }
    }
}
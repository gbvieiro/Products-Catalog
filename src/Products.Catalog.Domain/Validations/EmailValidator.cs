using System.Text.RegularExpressions;

namespace Products.Catalog.Domain.Validations
{
    public class EmailValidator
    {
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
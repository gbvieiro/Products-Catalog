namespace Products.Catalog.Infra.Authentication
{
    /// <summary>
    /// Provide authentication methods.
    /// </summary>
    public interface IAuthenticationService
    {
        Task<string> GenerateToken(AuthenticationModel userDto);
    }
}
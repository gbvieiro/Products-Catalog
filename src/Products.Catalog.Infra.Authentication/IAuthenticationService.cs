namespace Products.Catalog.Infra.Authentication
{
    /// <summary>
    /// Provide authentication methods.
    /// </summary>
    public interface IAuthenticationService
    {
        Task<dynamic?> AuthenticateUser(AuthenticationModel userDto);
    }
}
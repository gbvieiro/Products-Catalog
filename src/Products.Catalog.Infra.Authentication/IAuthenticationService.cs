namespace Products.Catalog.Infra.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> GenerateToken(AuthenticationModel userDto);
    }
}
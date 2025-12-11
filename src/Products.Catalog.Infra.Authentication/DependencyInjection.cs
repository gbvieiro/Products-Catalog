using Microsoft.Extensions.DependencyInjection;
using Products.Catalog.Infra.Authentication;

namespace Product.Catalog.Infra.IOC;

public static class DependencyInjection
{
    public static IServiceCollection RegisterAuthentication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        
        return services;
    }
}
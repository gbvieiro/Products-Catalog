using Microsoft.Extensions.DependencyInjection;

namespace Products.Catalog.Application.AutoMapper;

public static class MapperServiceRegistration
{
    public static void AddMapperService(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<CustomProfile>();
        });
    }
}
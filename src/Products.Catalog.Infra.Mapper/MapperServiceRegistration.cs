using Microsoft.Extensions.DependencyInjection;

namespace Products.Catalog.Infra.Mapper
{
    public static class MapperServiceRegistration
    {
        public static void AddMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CustomProfile));
        }
    }
}
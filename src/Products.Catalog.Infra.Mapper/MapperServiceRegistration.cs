using Microsoft.Extensions.DependencyInjection;

namespace Products.Catalog.Infra.Mapper
{
    /// <summary>
    /// This class could be used to register a Auto Mapper service on a list of services.
    /// </summary>
    public static class MapperServiceRegistration
    {
        /// <summary>
        /// Add mapper service to current services.
        /// </summary>
        /// <param name="services">Current services.</param>
        public static void AddMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CustomProfile));
        }
    }
}
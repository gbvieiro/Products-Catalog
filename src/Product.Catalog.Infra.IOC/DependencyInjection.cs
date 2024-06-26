using Microsoft.Extensions.DependencyInjection;
using Product.Catalog.Infra.Data.Repositories;
using Products.Catalog.Application.Services.Books;
using Products.Catalog.Application.Services.Orders;
using Products.Catalog.Application.Services.Stocks;
using Products.Catalog.Application.Services.Users;
using Products.Catalog.Domain.RepositoriesInterfaces;
using Products.Catalog.Domain.Services.Orders;
using Products.Catalog.Infra.Authentication;

namespace Product.Catalog.Infra.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IBooksAppService, BooksAppService>();
            services.AddScoped<IOrdersAppService, OrdersAppService>();
            services.AddScoped<IStocksAppService, StocksAppService>();
            services.AddScoped<IUsersAppService, UsersAppService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOrderDomainService, OrderDomainService>();
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<IOrderRepository, OrdersRepository>();
            services.AddScoped<IStocksRepository, StoksRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }
    }
}
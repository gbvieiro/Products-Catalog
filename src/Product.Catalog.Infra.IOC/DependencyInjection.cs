﻿using Microsoft.Extensions.DependencyInjection;
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
    /// <summary>
    /// This class is responsable by configure the dependency injection in a list of services.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Register Dependency Injection Infra for the current services.
        /// </summary>
        /// <param name="services">Current services.</param>
        /// <returns>Current services configured.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Application services.
            services.AddScoped<IBooksAppService, BooksAppService>();
            services.AddScoped<IOrdersAppService, OrdersAppService>();
            services.AddScoped<IStocksAppService, StocksAppService>();
            services.AddScoped<IUsersAppService, UsersAppService>();

            // Authentication services
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            // Domain services
            services.AddScoped<IOrderDomainService, OrderDomainService>();

            // Repositories.
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<IOrderRepository, OrdersRepository>();
            services.AddScoped<IStocksRepository, StoksRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            return services;
        }
    }
}
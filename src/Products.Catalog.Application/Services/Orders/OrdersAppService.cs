﻿using Products.Catalog.Application.DTOs;

namespace Products.Catalog.Application.Services.Orders
{
    public class OrdersAppService : IOrdersAppService
    {
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(OrderDto book)
        {
            throw new NotImplementedException();
        }
    }
}
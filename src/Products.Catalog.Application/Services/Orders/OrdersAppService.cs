using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Products.Catalog.Application.Services.Orders
{
    /// <summary>
    /// Provide order domain user cases.
    /// </summary>
    public class OrdersAppService(IOrderRepository orderRepository, IMapper mapper) : IOrdersAppService
    {
        /// <summary>
        /// A order repository interface.
        /// </summary>
        private readonly IOrderRepository _orderRepository = orderRepository;
        
        /// <summary>
        /// A mapper service.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc/>
        public Task DeleteAsync(Guid id) => _orderRepository.DeleteAsync(id);
        
        /// <inheritdoc/>
        public async Task<List<OrderDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            var orders = await _orderRepository.GetAllAsync(filtertext, skip, take);
            return _mapper.Map<List<OrderDto>>(orders.ToList());
        }

        /// <inheritdoc/>
        public async Task<OrderDto?> GetAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            return order != null ? _mapper.Map<OrderDto>(order) : default;
        }

        /// <inheritdoc/>
        public Task SaveAsync(OrderDto orderDto)
        {
            ArgumentNullException.ThrowIfNull(orderDto);
            orderDto.GenerateId();
            var order = _mapper.Map<Order>(orderDto);
            return _orderRepository.SaveAsync(order);
        }
    }
}
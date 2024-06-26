using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces;
using Products.Catalog.Domain.Services.Orders;

namespace Products.Catalog.Application.Services.Orders
{
    public class OrdersAppService(
        IOrderRepository orderRepository,
        IOrderDomainService orderDomainService,
        IMapper mapper
    ) : IOrdersAppService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        private readonly IOrderDomainService _orderDomainService = orderDomainService;

        private readonly IMapper _mapper = mapper;

        public async Task<string> CancelAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);

            if(order == null)
            {
                return $"Could not found order {id}";
            }

            await _orderDomainService.CancelOrderAsync(order);

            return "Order was canceled!";
        }

        public Task DeleteAsync(Guid id) => _orderRepository.DeleteAsync(id);
        
        public async Task<List<OrderDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            var orders = await _orderRepository.GetAllAsync(filtertext, skip, take);
            return _mapper.Map<List<OrderDto>>(orders.ToList());
        }

        public async Task<List<OrderDto>> GetAllAsync(string filtertext, int skip, int take, Guid userId)
        {
            var orders = await _orderRepository.GetAllAsync(filtertext, skip, take, userId);
            return _mapper.Map<List<OrderDto>>(orders.ToList());
        }

        public async Task<OrderDto?> GetAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            return order != null ? _mapper.Map<OrderDto>(order) : default;
        }

        public async Task SaveAsync(OrderDto orderDto)
        {
            ArgumentNullException.ThrowIfNull(orderDto);

            orderDto.GenerateId();
            var order = _mapper.Map<Order>(orderDto);

            // Process a new order.
            await _orderDomainService.ProcessNewOrderAsync(order);
        }
    }
}
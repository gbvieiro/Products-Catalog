using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Interfaces;

namespace Products.Catalog.Application.Services.Orders;

public class OrdersAppService(
    IRepository<Order> orderRepository,
    IMapper mapper
) : IOrdersAppService
{
    private readonly IRepository<Order> _orderRepository = orderRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> CreateAsync(OrderDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var order = _mapper.Map<Order>(dto);
        await _orderRepository.CreateAsync(order);
        return order.Id;
    }

    public async Task<OrderDto?> ReadAsync(Guid id)
    {
        var order = await _orderRepository.ReadAsync(id);
        return order != null ? _mapper.Map<OrderDto>(order) : default;
    }

    public async Task UpdateAsync(Guid id, OrderDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var entity = await _orderRepository.ReadAsync(id);
        if (entity is not null)
        {
            var order = _mapper.Map<Order>(dto);
            await _orderRepository.UpdateAsync(id, order);
        }
    }

    public Task DeleteAsync(Guid id) => _orderRepository.DeleteAsync(id);

    public async Task<IReadOnlyCollection<OrderDto>> FindAsync(string filterText)
    {
        var orders = await _orderRepository.FindAsync(filterText, 0, 100);
        return _mapper.Map<List<OrderDto>>(orders.ToList());
    }

    public async Task<string> CancelAsync(Guid id)
    {
        var order = await _orderRepository.ReadAsync(id);

        if (order == null)
        {
            return $"Could not found order {id}";
        }

        return "Order was canceled!";
    }

    public async Task<List<OrderDto>> GetAllAsync(string filtertext, int skip, int take, Guid userId)
    {
        var allOrders = await _orderRepository.FindAsync(string.Empty, 0, int.MaxValue);
        var orders = allOrders
            .Where(o => o.CustomerId == userId)
            .Skip(skip)
            .Take(take);

        if (!string.IsNullOrWhiteSpace(filtertext))
        {
            filtertext = filtertext.ToLower();
            orders = orders.Where(o =>
                o.Status.ToString().ToLower().Contains(filtertext) ||
                o.Id.ToString().ToLower().Contains(filtertext)
            );
        }

        return _mapper.Map<List<OrderDto>>(orders.ToList());
    }
}
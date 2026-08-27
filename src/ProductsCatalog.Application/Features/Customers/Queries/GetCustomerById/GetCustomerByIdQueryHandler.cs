using AutoMapper;
using MediatR;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.Id, cancellationToken);
        return customer is null ? null : mapper.Map<CustomerDto>(customer);
    }
}

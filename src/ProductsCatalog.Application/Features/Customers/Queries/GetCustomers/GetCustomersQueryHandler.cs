using AutoMapper;
using MediatR;
using ProductsCatalog.Application.Common.Models;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.Specifications;

namespace ProductsCatalog.Application.Features.Customers.Queries.GetCustomers;

public sealed class GetCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    : IRequestHandler<GetCustomersQuery, PagedResult<CustomerDto>>
{
    public async Task<PagedResult<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var spec = new CustomersFilterSpecification(request.Filter, request.Skip, request.Take);

        var customers = await customerRepository.ListAsync(spec, cancellationToken);
        var total = await customerRepository.CountAsync(spec, cancellationToken);

        return new PagedResult<CustomerDto>(mapper.Map<List<CustomerDto>>(customers), total, request.Skip, request.Take);
    }
}

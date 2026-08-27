using MediatR;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.ValueObjects;

namespace ProductsCatalog.Application.Features.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<CreateCustomerCommand, Guid>
{
    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Customer(request.Name, new Email(request.Email));

        await customerRepository.AddAsync(customer, cancellationToken);

        return customer.Id;
    }
}

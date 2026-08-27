using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;
using ProductsCatalog.Domain.ValueObjects;

namespace ProductsCatalog.Application.Features.Customers.Commands.UpdateCustomer;

public sealed class UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<UpdateCustomerCommand, Unit>
{
    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.Id);

        customer.Update(request.Name, new Email(request.Email));
        customerRepository.Update(customer);

        return Unit.Value;
    }
}

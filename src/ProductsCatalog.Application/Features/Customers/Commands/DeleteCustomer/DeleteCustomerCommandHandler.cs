using MediatR;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Repositories;

namespace ProductsCatalog.Application.Features.Customers.Commands.DeleteCustomer;

public sealed class DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
    : IRequestHandler<DeleteCustomerCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.Id);

        customerRepository.Remove(customer);

        return Unit.Value;
    }
}

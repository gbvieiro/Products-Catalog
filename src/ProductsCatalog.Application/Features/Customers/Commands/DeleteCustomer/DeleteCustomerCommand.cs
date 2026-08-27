using MediatR;
using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Customers.Commands.DeleteCustomer;

public sealed record DeleteCustomerCommand(Guid Id) : ICommand<Unit>;

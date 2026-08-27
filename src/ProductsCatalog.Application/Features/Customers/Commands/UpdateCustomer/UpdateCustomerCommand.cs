using MediatR;
using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Customers.Commands.UpdateCustomer;

public sealed record UpdateCustomerCommand(Guid Id, string Name, string Email) : ICommand<Unit>;

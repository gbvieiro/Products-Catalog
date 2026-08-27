using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(string Name, string Email) : ICommand<Guid>;

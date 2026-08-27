using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Customers.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid Id) : IQuery<CustomerDto?>;

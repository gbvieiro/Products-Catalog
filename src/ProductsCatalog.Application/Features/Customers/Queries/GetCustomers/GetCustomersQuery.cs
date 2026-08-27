using ProductsCatalog.Application.Common.Messaging;
using ProductsCatalog.Application.Common.Models;

namespace ProductsCatalog.Application.Features.Customers.Queries.GetCustomers;

public sealed record GetCustomersQuery(string? Filter = null, int Skip = 0, int Take = 20) : IQuery<PagedResult<CustomerDto>>;

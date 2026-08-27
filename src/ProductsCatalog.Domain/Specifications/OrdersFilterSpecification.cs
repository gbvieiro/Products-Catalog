using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Domain.Specifications;

public sealed class OrdersFilterSpecification : BaseSpecification<Order>
{
    public OrdersFilterSpecification(Guid? customerId, string? filterText, int skip, int take)
    {
        if (customerId.HasValue && customerId.Value != Guid.Empty)
        {
            var id = customerId.Value;
            AddCriteria(o => o.CustomerId == id);
        }

        if (!string.IsNullOrWhiteSpace(filterText))
        {
            AddCriteria(o =>
                o.Status.ToString().Contains(filterText) ||
                o.Id.ToString().Contains(filterText));
        }

        AddOrderBy(o => o.CreatedAt, descending: true);
        ApplyPaging(skip, take);
    }
}

using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Domain.Specifications;

public sealed class CustomersFilterSpecification : BaseSpecification<Customer>
{
    public CustomersFilterSpecification(string? filterText, int skip, int take)
    {
        if (!string.IsNullOrWhiteSpace(filterText))
        {
            AddCriteria(c =>
                c.Name.Contains(filterText) ||
                c.Email.Address.Contains(filterText));
        }

        AddOrderBy(c => c.CreatedAt, descending: true);
        ApplyPaging(skip, take);
    }
}

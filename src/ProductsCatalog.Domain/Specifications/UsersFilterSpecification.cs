using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Domain.Specifications;

public sealed class UsersFilterSpecification : BaseSpecification<User>
{
    public UsersFilterSpecification(string? filterText, int skip, int take)
    {
        if (!string.IsNullOrWhiteSpace(filterText))
        {
            AddCriteria(u =>
                u.Role.Contains(filterText) ||
                u.Email.Address.Contains(filterText));
        }

        AddOrderBy(u => u.CreatedAt, descending: true);
        ApplyPaging(skip, take);
    }
}

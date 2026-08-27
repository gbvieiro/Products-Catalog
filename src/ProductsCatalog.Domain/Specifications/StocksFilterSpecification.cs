using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Domain.Specifications;

public sealed class StocksFilterSpecification : BaseSpecification<Stock>
{
    public StocksFilterSpecification(string? filterText, int skip, int take)
    {
        if (!string.IsNullOrWhiteSpace(filterText))
        {
            AddCriteria(s =>
                s.Id.ToString().Contains(filterText) ||
                s.BookId.ToString().Contains(filterText));
        }

        AddOrderBy(s => s.CreatedAt, descending: true);
        ApplyPaging(skip, take);
    }
}

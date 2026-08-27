using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Domain.Specifications;

public sealed class BooksFilterSpecification : BaseSpecification<Book>
{
    public BooksFilterSpecification(string? filterText, int skip, int take)
    {
        if (!string.IsNullOrWhiteSpace(filterText))
        {
            AddCriteria(b =>
                b.Title.Contains(filterText) ||
                b.Author.Contains(filterText) ||
                b.Id.ToString().Contains(filterText));
        }

        AddOrderBy(b => b.CreatedAt, descending: true);
        ApplyPaging(skip, take);
    }
}

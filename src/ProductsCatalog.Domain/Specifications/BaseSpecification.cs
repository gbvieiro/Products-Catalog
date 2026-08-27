using System.Linq.Expressions;

namespace ProductsCatalog.Domain.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public List<Expression<Func<T, bool>>> Criteria { get; } = [];
    public List<Expression<Func<T, object>>> Includes { get; } = [];
    public List<(Expression<Func<T, object>> KeySelector, bool Descending)> OrderBy { get; } = [];
    public int Skip { get; private set; }
    public int Take { get; private set; }
    public bool IsPagingEnabled { get; private set; }

    protected void AddCriteria(Expression<Func<T, bool>> criteria) => Criteria.Add(criteria);

    protected void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);

    protected void AddOrderBy(Expression<Func<T, object>> keySelector, bool descending = false) =>
        OrderBy.Add((keySelector, descending));

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}

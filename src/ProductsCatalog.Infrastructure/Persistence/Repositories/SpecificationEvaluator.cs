using Microsoft.EntityFrameworkCore;
using ProductsCatalog.Domain.Specifications;

namespace ProductsCatalog.Infrastructure.Persistence.Repositories;

/// <summary>Traduz uma ISpecification&lt;T&gt; do Domain em uma query LINQ/EF Core de fato.</summary>
public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(
        IQueryable<T> inputQuery,
        ISpecification<T> specification,
        bool evaluateCriteriaOnly = false) where T : class
    {
        var query = inputQuery;

        foreach (var criteria in specification.Criteria)
        {
            query = query.Where(criteria);
        }

        if (evaluateCriteriaOnly)
        {
            return query;
        }

        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        if (specification.OrderBy.Count != 0)
        {
            IOrderedQueryable<T>? orderedQuery = null;

            foreach (var (keySelector, descending) in specification.OrderBy)
            {
                orderedQuery = orderedQuery is null
                    ? (descending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector))
                    : (descending ? orderedQuery.ThenByDescending(keySelector) : orderedQuery.ThenBy(keySelector));
            }

            query = orderedQuery!;
        }

        if (specification.IsPagingEnabled)
        {
            query = query.Skip(specification.Skip).Take(specification.Take);
        }

        return query;
    }
}

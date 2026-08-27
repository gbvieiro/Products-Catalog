using System.Linq.Expressions;

namespace ProductsCatalog.Domain.Specifications;

/// <summary>
/// Descreve "o que" buscar (filtros, ordenacao, paginacao) sem que o Domain
/// precise conhecer EF Core ou qualquer tecnologia de acesso a dados.
/// Quem "traduz" a Specification em uma query real e o
/// SpecificationEvaluator, na Infrastructure.
/// </summary>
public interface ISpecification<T>
{
    List<Expression<Func<T, bool>>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<(Expression<Func<T, object>> KeySelector, bool Descending)> OrderBy { get; }
    int Skip { get; }
    int Take { get; }
    bool IsPagingEnabled { get; }
}

using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Domain.Specifications;

public sealed class UsersFilterSpecification : BaseSpecification<User>
{
    public UsersFilterSpecification(string? filterText, int skip, int take)
    {
        if (!string.IsNullOrWhiteSpace(filterText))
        {
            // Role agora e um enum (ERole), nao mais texto livre - nao da mais
            // pra fazer Contains(filterText) nele (e nem seria um match
            // confiavel via SQL comparando o int armazenado com uma string).
            // O filtro de texto passa a valer so para o email.
            AddCriteria(u => u.Email.Address.Contains(filterText));
        }

        AddOrderBy(u => u.CreatedAt, descending: true);
        ApplyPaging(skip, take);
    }
}

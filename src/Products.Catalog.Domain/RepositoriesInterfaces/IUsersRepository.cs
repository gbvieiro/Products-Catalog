using Products.Catalog.Domain.Entities.Users;
using Products.Catalog.Domain.RepositoriesInterfaces.Common;

namespace Products.Catalog.Domain.RepositoriesInterfaces
{
    public interface IUsersRepository : IRepository<User, Guid>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
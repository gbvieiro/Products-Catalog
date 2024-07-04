using Products.Catalog.Domain.Entities.Users;
using Products.Catalog.Domain.Interfaces.Common;

namespace Products.Catalog.Domain.Interfaces
{
    public interface IUsersRepository : IRepository<User, Guid>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
using Products.Catalog.Domain.Entities.Users;
using Products.Catalog.Domain.RepositoriesInterfaces.Common;

namespace Products.Catalog.Domain.RepositoriesInterfaces
{
    public interface IUsersRepository : IRepository<User, Guid>
    {
        /// <summary>
        /// Get a user by his email.
        /// </summary>
        /// <param name="email">A email.</param>
        /// <returns>A user when email is register.</returns>
        Task<User?> GetByEmailAsync(string email);
    }
}
using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}

using Products.Catalog.Domain.Entities;

namespace Products.Catalog.Domain.Interfaces
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User?> GetAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync(string filter, int skip, int take);
        Task SaveAsync(User entity);
        Task<User?> GetByEmailAsync(string email);
    }
}
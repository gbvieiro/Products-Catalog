using Product.Catalog.Infra.Data.Database;
using Products.Catalog.Domain.Entities.Users;
using Products.Catalog.Domain.Interfaces;

namespace Product.Catalog.Infra.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public Task DeleteAsync(Guid id)
        {
            Context.Users = new List<User>(Context.Users.Where(x => x.Id != id));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<User>> GetAllAsync(string filter, int skip, int take)
        {
            if (Context.Users == null)
            {
                throw new Exception("Context is not initialized.");
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = filter.ToLower();

                var users = Context.Users.Where(x =>
                    x.Email.ToString().ToLower().Contains(filter)
                ).Skip(skip).Take(take);

                return Task.FromResult(users);
            }

            return Task.FromResult(Context.Users.Skip(skip).Take(take));
        }

        public Task<User?> GetAsync(Guid id) =>
            Task.FromResult(Context.Users.FirstOrDefault(x => x.Id == id));

        public Task<User?> GetByEmailAsync(string email) =>
            Task.FromResult(Context.Users.FirstOrDefault(x => x.Email == email));

        public Task SaveAsync(User entity)
        {
            var savedEntityIndex = Context.Users.FindIndex(x => x.Id == entity.Id);
            if (savedEntityIndex < 0)
            {
                Context.Users.Add(entity);
            }
            else
            {
                Context.Users[savedEntityIndex] = entity;
            }

            return Task.CompletedTask;
        }
    }
}
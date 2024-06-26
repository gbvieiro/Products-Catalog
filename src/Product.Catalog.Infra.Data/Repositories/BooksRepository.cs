using Product.Catalog.Infra.Data.Database;
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Product.Catalog.Infra.Data.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        public Task DeleteAsync(Guid id)
        {
            Context.Books = new List<Book>(Context.Books.Where(x => x.Id != id));
            return Task.CompletedTask;
        }

        public Task<Book?> GetAsync(Guid id) =>
            Task.FromResult(Context.Books.FirstOrDefault(x => x.Id == id));

        public Task<IEnumerable<Book>> GetAllAsync(string filter, int skip, int take)
        {
            if (Context.Books == null)
            {
                throw new Exception("Context is not initialized.");
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = filter.ToLower();

                var books = Context.Books.Where(x =>
                    x.Genre.ToString().ToLower().Contains(filter) ||
                    x.Title.ToLower().Contains(filter) ||
                    x.Author.ToLower().Contains(filter) ||
                    x.Id.ToString().ToLower().Contains(filter)
                ).Skip(skip).Take(take);

                return Task.FromResult(books);
            }

            return Task.FromResult(Context.Books.Skip(skip).Take(take));
        }

        public Task SaveAsync(Book entity)
        {
            var savedEntityIndex = Context.Books.FindIndex(x => x.Id == entity.Id);
            if (savedEntityIndex < 0)
            {
                Context.Books.Add(entity);
            }
            else
            {
                Context.Books[savedEntityIndex] = entity;
            }

            return Task.CompletedTask;
        }

        public Task<double> GetBookPrice(Guid id)
        {
            if (Context.Books == null)
            {
                throw new Exception("Context is not initialized.");
            }

            var index = Context.Books.FindIndex(x => x.Id == id);

            if (index < 0) {
                Task.FromResult(double.NaN);
            }

            return Task.FromResult(Context.Books[index].Price);
        }
    }
}
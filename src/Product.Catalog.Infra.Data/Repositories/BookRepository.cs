using Product.Catalog.Infra.Data.Database;
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.RepositoriesInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Catalog.Infra.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        /// <inheritdoc/>
        public Task DeleteAsync(Guid id)
        {
            Context.Books = new List<Book>(Context.Books.Where(x => x.Id != id));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<Book?> GetAsync(Guid id) =>
            Task.FromResult(Context.Books.FirstOrDefault(x => x.Id == id));

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
    }
}
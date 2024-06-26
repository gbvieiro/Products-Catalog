using Products.Catalog.Domain.Entities.Base;
using Products.Catalog.Domain.Validations;

namespace Products.Catalog.Domain.Entities.Books
{
    public class Book : Product
    {
        public string Title { get; private set; }

        public string Author { get; private set; }

        public BookGenre Genre { get; private set; }

        public Book(Guid id, double price, string title, string author, BookGenre genre) : base(id, price)
        {
            Title = title;
            Author = author;
            Genre = genre;

            ValidateBookDomain();
        }

        private void ValidateBookDomain()
        {
            DomainExceptionValidation.When(string.IsNullOrEmpty(Title), "title is required.");
            DomainExceptionValidation.When(Title.Length > 30, "Invalid title, too long, maximum 30 characters.");

            DomainExceptionValidation.When(string.IsNullOrEmpty(Author), "Author is required.");
            DomainExceptionValidation.When(Author.Length < 3, "Invalid author name, too short, minimum 3 characters.");
            DomainExceptionValidation.When(Author.Length > 30, "Invalid author name, too long, maximum 3 characters.");
        }
    }
}
using Products.Catalog.Domain.Enums;
using Products.Catalog.Domain.Validations;
using Shared.Entities;

namespace Products.Catalog.Domain.Entities;

public class Book : Product
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public EBookGenre Genre { get; private set; }

    protected Book() : base()
    {
        Title = string.Empty;
        Author = string.Empty;
    }

    public Book(double price, string title, string author, EBookGenre genre) : base(price)
    {
        Title = title;
        Author = author;
        Genre = genre;

        ValidateBookDomain();
    }

    private void ValidateBookDomain()
    {
        DomainException.When(string.IsNullOrEmpty(Title), "title is required.");
        DomainException.When(Title.Length > 30, "Invalid title, too long, maximum 30 characters.");

        DomainException.When(string.IsNullOrEmpty(Author), "Author is required.");
        DomainException.When(Author.Length < 3, "Invalid author name, too short, minimum 3 characters.");
        DomainException.When(Author.Length > 30, "Invalid author name, too long, maximum 3 characters.");
    }
}
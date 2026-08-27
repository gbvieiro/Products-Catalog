using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.Exceptions;

namespace ProductsCatalog.Domain.Entities;

public class Book : Product
{
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public EBookGenre Genre { get; private set; }

    protected Book()
    {
    }

    public Book(double price, string title, string author, EBookGenre genre) : base(price)
    {
        Title = title;
        Author = author;
        Genre = genre;

        Validate();
    }

    public void Update(double price, string title, string author, EBookGenre genre)
    {
        Price = price;
        Title = title;
        Author = author;
        Genre = genre;

        Validate();
        Touch();
    }

    private void Validate()
    {
        DomainException.When(string.IsNullOrEmpty(Title), "Title is required.");
        DomainException.When(Title.Length > 30, "Invalid title, too long, maximum 30 characters.");

        DomainException.When(string.IsNullOrEmpty(Author), "Author is required.");
        DomainException.When(Author.Length < 3, "Invalid author name, too short, minimum 3 characters.");
        DomainException.When(Author.Length > 30, "Invalid author name, too long, maximum 30 characters.");

        DomainException.When(Price < 0, "Price cannot be negative.");
    }
}

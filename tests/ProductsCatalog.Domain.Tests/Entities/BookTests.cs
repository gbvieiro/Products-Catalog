using FluentAssertions;
using ProductsCatalog.Domain.Entities;
using ProductsCatalog.Domain.Enums;
using ProductsCatalog.Domain.Exceptions;
using Xunit;

namespace ProductsCatalog.Domain.Tests.Entities;

public class BookTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesBook()
    {
        var book = new Book(29.9, "Clean Architecture", "Robert C. Martin", EBookGenre.NonFiction);

        book.Title.Should().Be("Clean Architecture");
        book.Author.Should().Be("Robert C. Martin");
        book.Price.Should().Be(29.9);
        book.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData("", "Robert C. Martin")]
    [InlineData("This title has definitely way more than thirty characters", "Robert C. Martin")]
    [InlineData("Clean Architecture", "Bo")]
    public void Constructor_WithInvalidData_ThrowsDomainException(string title, string author)
    {
        var act = () => new Book(29.9, title, author, EBookGenre.NonFiction);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Update_WithValidData_ChangesFieldsAndTouchesUpdatedAt()
    {
        var book = new Book(29.9, "Clean Architecture", "Robert C. Martin", EBookGenre.NonFiction);
        var originalUpdatedAt = book.UpdatedAt;

        book.Update(39.9, "Clean Code", "Robert C. Martin", EBookGenre.Education);

        book.Price.Should().Be(39.9);
        book.Title.Should().Be("Clean Code");
        book.Genre.Should().Be(EBookGenre.Education);
        book.UpdatedAt.Should().BeOnOrAfter(originalUpdatedAt);
    }
}

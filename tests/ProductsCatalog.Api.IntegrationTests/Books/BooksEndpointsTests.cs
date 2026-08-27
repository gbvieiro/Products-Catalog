using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProductsCatalog.Api.IntegrationTests.Common;
using ProductsCatalog.Application.Features.Books;
using ProductsCatalog.Application.Features.Books.Commands.CreateBook;
using ProductsCatalog.Domain.Enums;
using Xunit;

namespace ProductsCatalog.Api.IntegrationTests.Books;

public class BooksEndpointsTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
{
    private HttpClient _client = null!;

    public async Task InitializeAsync() => _client = await factory.CreateAuthenticatedClientAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task CreateAndGetBook_ReturnsCreatedBook()
    {
        var command = new CreateBookCommand(29.9, "Clean Architecture", "Robert C. Martin", EBookGenre.NonFiction);

        var createResponse = await _client.PostAsJsonAsync("/api/books", command);
        createResponse.EnsureSuccessStatusCode();

        var bookId = await createResponse.Content.ReadFromJsonAsync<Guid>();
        bookId.Should().NotBeEmpty();

        var book = await _client.GetFromJsonAsync<BookDto>($"/api/books/{bookId}");

        book.Should().NotBeNull();
        book!.Title.Should().Be("Clean Architecture");
        book.Author.Should().Be("Robert C. Martin");
    }

    [Fact]
    public async Task CreateBook_WithInvalidData_ReturnsBadRequest()
    {
        var command = new CreateBookCommand(10, string.Empty, "AB", EBookGenre.Fiction);

        var response = await _client.PostAsJsonAsync("/api/books", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ReadAsync_WithUnknownId_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/books/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

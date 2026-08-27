using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProductsCatalog.Api.IntegrationTests.Common;
using ProductsCatalog.Application.Features.Books.Commands.CreateBook;
using ProductsCatalog.Application.Features.Customers.Commands.CreateCustomer;
using ProductsCatalog.Application.Features.Orders.Commands.CreateOrder;
using ProductsCatalog.Application.Features.Stocks;
using ProductsCatalog.Domain.Enums;
using Xunit;

namespace ProductsCatalog.Api.IntegrationTests.Orders;

/// <summary>Teste end-to-end do fluxo mais interessante do dominio: criar pedido reserva estoque, cancelar devolve.</summary>
public class OrdersEndpointsTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
{
    private HttpClient _client = null!;

    public async Task InitializeAsync() => _client = await factory.CreateAuthenticatedClientAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    private async Task<Guid> CreateBookAsync(string title)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/books", new CreateBookCommand(15, title, "Some Author", EBookGenre.NonFiction));
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    /// <summary>CreateOrderCommand exige um Customer existente (ver CreateOrderCommandHandler).</summary>
    private async Task<Guid> CreateCustomerAsync(string name)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/customers", new CreateCustomerCommand(name, $"{Guid.NewGuid()}@example.com"));
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    [Fact]
    public async Task FullOrderLifecycle_ReservesAndReplenishesStock()
    {
        var bookId = await CreateBookAsync("Domain-Driven Design");
        var customerId = await CreateCustomerAsync("Alice");

        (await _client.PutAsJsonAsync($"/api/stocks/book/{bookId}/add", new { Quantity = 10 }))
            .EnsureSuccessStatusCode();

        var createOrderResponse = await _client.PostAsJsonAsync(
            "/api/orders", new CreateOrderCommand(customerId, [new CreateOrderItemRequest(bookId, 3)]));
        createOrderResponse.EnsureSuccessStatusCode();
        var orderId = await createOrderResponse.Content.ReadFromJsonAsync<Guid>();

        var stockAfterOrder = await _client.GetFromJsonAsync<CompleteStockDto>($"/api/stocks/book/{bookId}");
        stockAfterOrder!.Quantity.Should().Be(7);

        (await _client.PutAsync($"/api/orders/{orderId}/cancel", null)).EnsureSuccessStatusCode();

        var stockAfterCancel = await _client.GetFromJsonAsync<CompleteStockDto>($"/api/stocks/book/{bookId}");
        stockAfterCancel!.Quantity.Should().Be(10);
    }

    [Fact]
    public async Task CreateOrder_WithoutEnoughStock_ReturnsBadRequest()
    {
        var bookId = await CreateBookAsync("Refactoring");
        var customerId = await CreateCustomerAsync("Bob");

        var response = await _client.PostAsJsonAsync(
            "/api/orders", new CreateOrderCommand(customerId, [new CreateOrderItemRequest(bookId, 1)]));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

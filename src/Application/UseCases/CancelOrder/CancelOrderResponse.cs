using System.Net;

namespace Products.Catalog.Application.UseCases.CancelOrder;

public sealed record CancelOrderResponse
{
    public string? Message { get; init; }
    public HttpStatusCode StatusCode { get; init; } 
}
namespace ProductsCatalog.Application.Features.Customers;

// Nao implementa IMapFrom<Customer> de proposito: o mapeamento Customer ->
// CustomerDto precisa "achatar" Email (Value Object) para string - mesmo
// motivo do UserDto. Esse mapeamento explicito fica no CustomerMappingProfile.
public sealed record CustomerDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

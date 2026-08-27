using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Application.Features.Users;

// Nao implementa IMapFrom<User> de proposito: o mapeamento User -> UserDto
// precisa "achatar" Email (Value Object) para string e NUNCA deve expor
// PasswordHash. Esse mapeamento explicito fica no MappingProfile.
public sealed record UserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public ERole Role { get; init; }
    public DateTime CreatedAt { get; init; }
}

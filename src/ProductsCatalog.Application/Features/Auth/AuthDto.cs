using ProductsCatalog.Domain.Enums;

namespace ProductsCatalog.Application.Features.Auth;

public sealed record AuthenticatedUserDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public ERole Role { get; init; }
}

public sealed record LoginResult
{
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAtUtc { get; init; }
    public required AuthenticatedUserDto User { get; init; }
}

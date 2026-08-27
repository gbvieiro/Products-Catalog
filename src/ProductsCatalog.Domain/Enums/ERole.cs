namespace ProductsCatalog.Domain.Enums;

/// <summary>
/// Administrator pode tudo. Seller so pode gerar (criar) e ver pedidos -
/// ver [Authorize(Roles = ...)] nos controllers e a ProtectedRoute no
/// frontend, que usam este mesmo enum para filtrar endpoints e menus.
/// </summary>
public enum ERole
{
    Administrator = 1,
    Seller = 2
}

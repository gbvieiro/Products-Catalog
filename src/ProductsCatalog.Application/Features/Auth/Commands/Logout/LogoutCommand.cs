using MediatR;
using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Features.Auth.Commands.Logout;

/// <summary>
/// JWT e stateless: o "logout" de verdade acontece no client (descartar o
/// token guardado - ver shared/auth/AuthContext.tsx no frontend). Nao ha
/// nada para invalidar no servidor sem um mecanismo de revogacao (blacklist
/// de Jti em cache/banco, checado a cada request) - fica como extension
/// point (mesmo espirito de Messaging/Caching na Infrastructure). Este
/// endpoint existe para completar o fluxo pedido e como lugar natural para
/// plugar essa revogacao no futuro.
/// </summary>
public sealed record LogoutCommand : ICommand<Unit>;

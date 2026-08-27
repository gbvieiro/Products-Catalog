namespace ProductsCatalog.Application.Common.Exceptions;

/// <summary>Credenciais invalidas ao logar. Mapeada para 401 pelo ExceptionHandlingMiddleware.</summary>
public class UnauthorizedException(string message) : Exception(message)
{
}

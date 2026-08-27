using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProductsCatalog.Application.Common.Exceptions;
using ProductsCatalog.Domain.Exceptions;
using ValidationException = ProductsCatalog.Application.Common.Exceptions.ValidationException;

namespace ProductsCatalog.Api.Middleware;

/// <summary>
/// Ponto unico de traducao de excecao -> resposta HTTP. Assim os
/// controllers ficam livres de try/catch e as camadas internas so lancam
/// excecoes de dominio/aplicacao "puras".
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "Validation error";
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Extensions["errors"] = validationException.Errors;
                break;

            case NotFoundException notFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                problemDetails.Title = "Resource not found";
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Detail = notFoundException.Message;
                break;

            case UnauthorizedException unauthorizedException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                problemDetails.Title = "Unauthorized";
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Detail = unauthorizedException.Message;
                break;

            case DomainException domainException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "Business rule violation";
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Detail = domainException.Message;
                break;

            default:
                logger.LogError(exception, "Unhandled exception");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Title = "An unexpected error occurred";
                problemDetails.Status = context.Response.StatusCode;
                break;
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}

using MediatR;
using ProductsCatalog.Application.Common.Interfaces;
using ProductsCatalog.Application.Common.Messaging;

namespace ProductsCatalog.Application.Common.Behaviors;

/// <summary>
/// Coracao da separacao Command/Query em termos de persistencia: somente
/// quando a request e um ICommand chamamos IUnitOfWork.SaveChangesAsync,
/// e sempre em um unico lugar, apos o handler concluir com sucesso. Handlers
/// de Command nunca chamam SaveChangesAsync diretamente.
/// </summary>
public class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is ICommand)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return response;
    }
}

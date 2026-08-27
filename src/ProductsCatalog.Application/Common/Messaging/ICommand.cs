using MediatR;

namespace ProductsCatalog.Application.Common.Messaging;

/// <summary>
/// Marker nao-generico usado pelo UnitOfWorkBehavior para reconhecer,
/// em tempo de execucao, se a request em andamento e um Command (escreve)
/// ou uma Query (le) - so Commands disparam SaveChangesAsync ao final.
/// </summary>
public interface ICommand : IBaseRequest
{
}

/// <summary>Um Command e uma intencao de mudar o estado do sistema (a parte "C" do CQRS).</summary>
public interface ICommand<out TResponse> : IRequest<TResponse>, ICommand
{
}

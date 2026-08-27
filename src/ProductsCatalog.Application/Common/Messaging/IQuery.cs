using MediatR;

namespace ProductsCatalog.Application.Common.Messaging;

/// <summary>Uma Query e uma leitura pura, sem efeitos colaterais (a parte "Q" do CQRS).</summary>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}

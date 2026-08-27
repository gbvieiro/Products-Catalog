# Caching

Extension point para cache (ex: `IDistributedCache` com Redis) atras das
Queries mais custosas (ex: `GetBooksQuery`). Uma abordagem comum em CQRS e
um `CachingBehavior<TRequest,TResponse>` (pipeline do MediatR, igual aos que
ja existem em `Application/Common/Behaviors`) que so entra em acao quando
`TRequest` implementa uma interface marker `ICacheableQuery`.

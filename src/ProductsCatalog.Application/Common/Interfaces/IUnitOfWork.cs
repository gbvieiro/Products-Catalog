namespace ProductsCatalog.Application.Common.Interfaces;

/// <summary>
/// Port implementada pelo ApplicationDbContext na Infrastructure. Mantida
/// separada de IRepository para deixar explicito que "salvar" e uma
/// operacao de transacao unica, nao responsabilidade de cada repositorio.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

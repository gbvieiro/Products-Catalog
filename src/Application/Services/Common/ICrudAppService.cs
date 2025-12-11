using Products.Catalog.Application.DTOs;

namespace Products.Catalog.Application.Services.Common;

public interface ICrudAppService<DtoType> where DtoType : EntityDto
{
    Task<Guid> CreateAsync(DtoType dto);
    Task<DtoType?> ReadAsync(Guid id);
    Task UpdateAsync(Guid id, DtoType dto);
    Task DeleteAsync(Guid id);
    Task<IReadOnlyCollection<DtoType>> FindAsync(string filterText);
}
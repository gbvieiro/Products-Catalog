using Products.Catalog.Application.DTOs.Common;

namespace Products.Catalog.Application.Services.Common
{
    public interface ICrudAppService<DtoType> where DtoType : EntityDTO
    {
        Task<DtoType?> GetAsync(Guid id);

        Task<List<DtoType>> GetAllAsync(string filtertext, int skip, int take);

        Task SaveAsync(DtoType dto);

        Task DeleteAsync(Guid id);
    }
}
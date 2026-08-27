using AutoMapper;

namespace ProductsCatalog.Application.Common.Mappings;

/// <summary>
/// Convencao para reduzir boilerplate de CreateMap: qualquer DTO que
/// implemente IMapFrom&lt;TSource&gt; ganha automaticamente um mapeamento
/// TSource -> DTO registrado pelo MappingProfile.
/// </summary>
public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}

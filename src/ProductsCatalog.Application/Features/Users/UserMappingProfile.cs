using AutoMapper;
using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Application.Features.Users;

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address));
    }
}

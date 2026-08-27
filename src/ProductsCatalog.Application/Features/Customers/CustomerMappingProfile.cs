using AutoMapper;
using ProductsCatalog.Domain.Entities;

namespace ProductsCatalog.Application.Features.Customers;

public sealed class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address));
    }
}

using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.ValueObject;

namespace Products.Catalog.Application.AutoMapper;

public class CustomProfile : Profile
{
    public CustomProfile()
    {
        CreateMap<ProductDto, Shared.Entities.Product>().ReverseMap();
        CreateMap<BookDto, Book>().ReverseMap();
        CreateMap<OrderDto, Order>().ReverseMap();
        CreateMap<OrderItemDto, OrderItem>().ReverseMap();
        CreateMap<StockDto, Stock>().ReverseMap();

        CreateMap<UserDto, User>()
            .ConstructUsing(src => new User(new Email(src.Email), src.Password, src.Role))
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore());

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Address));
    }
}
using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Common;
using Products.Catalog.Application.DTOs.Stocks;
using Products.Catalog.Domain.Entities;

namespace Products.Catalog.Infra.Mapper
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<ProductDTO, Shared.Entities.Product>().ReverseMap();
            CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
            CreateMap<StockDto, Stock>().ReverseMap();
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>()
                .ForMember(x => x.Password, x => x.Ignore());
        }
    }
}
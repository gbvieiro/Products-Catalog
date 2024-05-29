using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Application.DTOs.Common;
using Products.Catalog.Domain.Entities.Base;
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Domain.Entities.Stocks;

namespace Products.Catalog.Infra.Mapper
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<OrderDto, Order>().ReverseMap();
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
            CreateMap<StockDto, Stock>().ReverseMap();
        }
    }
}
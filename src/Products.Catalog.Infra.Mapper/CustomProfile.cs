using AutoMapper;
using AutoMapper.Features;
using static System.Reflection.Metadata.BlobBuilder;
using System.Data;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Principal;
using Products.Catalog.Domain.Entities.Books;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities.Orders;
using Products.Catalog.Application.DTOs.Common;
using Products.Catalog.Domain.Entities.Base;

namespace Products.Catalog.Infra.Mapper
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<BookDTO, Book>().ReverseMap();
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<OrderItemDTO, OrderItem>().ReverseMap();
        }
    }
}
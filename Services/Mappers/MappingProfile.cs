using AutoMapper;
using Repositories.Entities;
using Services.Dtos;

namespace Services.Mappers;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));
        CreateMap<Category, CategoryDto>();
        CreateMap<Product, ProductDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));
        CreateMap<Cart, CartDto>();
        //CreateMap<CartItem, CartItemDto>()
        //    .ForMember(dest => dest.PricePerItem, opt => opt.MapFrom(src => src.Price))
        //    .ForMember(dest => dest.TotalItemPrice, opt => opt.MapFrom(src => src.Price * src.Quantity));

        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.PricePerItem, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.TotalItemPrice, opt => opt.MapFrom(src => src.Price * src.Quantity));

        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Cart != null ? src.Cart.TotalPrice : 0));
        CreateMap<Order, OrderDetailDto>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Cart != null ? src.Cart.TotalPrice : 0))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Cart.CartItems));
    }
}

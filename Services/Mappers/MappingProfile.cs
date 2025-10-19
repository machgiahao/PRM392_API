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

        CreateMap<Product, ProductDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));
    }
}

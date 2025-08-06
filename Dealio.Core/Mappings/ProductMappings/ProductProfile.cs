using AutoMapper;
using Dealio.Core.DTOs.Product;
using Dealio.Domain.Entities;


namespace Dealio.Core.Mappings.ProductMappings
{
    public partial class ProductProfile : Profile
    {
        public ProductProfile()
        {
            AddProductCommandMapping();
            UpdateProductProfile();

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(sre => sre.Images.Select(i => i.ImgUrl)))
                    .ReverseMap();

        }
    }
}

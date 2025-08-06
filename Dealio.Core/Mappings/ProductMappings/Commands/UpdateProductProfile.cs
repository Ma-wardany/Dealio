
using Dealio.Core.DTOs.Product;
using Dealio.Core.Features.Product.Commands.Models;

namespace Dealio.Core.Mappings.ProductMappings
{
    public partial class ProductProfile
    {
        public void UpdateProductProfile()
        {
            CreateMap<UpdateProductCommand, Domain.Entities.Product>()
                .ForMember(dest => dest.Id,     opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Domain.Entities.Product, ProductDto>()
                .ReverseMap();
        }
    }
}

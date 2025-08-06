
using Dealio.Core.DTOs.Product;
using Dealio.Core.Features.Product.Commands.Models;
using Dealio.Domain.Entities;

namespace Dealio.Core.Mappings.ProductMappings
{
    public partial class ProductProfile
    {
        public void AddProductCommandMapping()
        {
            CreateMap<AddProductCommand, Product>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Available"))
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ReverseMap();

        }
    }
}

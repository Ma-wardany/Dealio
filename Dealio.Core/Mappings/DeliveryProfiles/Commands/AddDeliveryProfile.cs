using Dealio.Core.Features.DeliveryProfile.Commands.Models;
using Dealio.Domain.Entities;

namespace Dealio.Core.Mappings.DeliveryProfiles
{
    public partial class DeliveryProfileProfile
    {
        public void CreateDeliveryProfileMappings()
        {
            CreateMap<CreateDeliveryProfileCommand, ApplicationUser>()
                .ForPath(dest => dest.DeliveryProfile.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(dest => dest.DeliveryProfile.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.DeliveryProfile.NationalId, opt => opt.MapFrom(src => src.NationalId))
                .ForPath(dest => dest.DeliveryProfile.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForPath(dest => dest.DeliveryProfile.Image, opt => opt.Ignore())
                .ForPath(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    City = src.City,
                    Region = src.Region,
                    Street = src.Street,
                    Latitude = 0,
                    Longitude = 0
                }));
        }
    }
}

using AutoMapper;
using Dealio.Core.Features.UserProfile.Commands.Models;

namespace Dealio.Core.Mappings.UserProfile
{
    public partial class UserProfileProfile
    {
        public void UserProfileCommandMappings()
        {
            CreateMap<ProfileCommandModel, Domain.Entities.UserProfile>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.Region, opt => opt.MapFrom(src => src.Region))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.Latitude, opt => opt.MapFrom(src => 0))
                .ForPath(dest => dest.Address.Longitude, opt => opt.MapFrom(src => 0))
                .ReverseMap();


            
        }
    }
}

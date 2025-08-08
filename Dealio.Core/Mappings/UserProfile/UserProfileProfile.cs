using AutoMapper;
using Dealio.Core.DTOs.UserProfileDto;

namespace Dealio.Core.Mappings.UserProfile
{
    public partial class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            UserProfileCommandMappings();

            CreateMap<Domain.Entities.UserProfile, UserProfileDto>()
                        .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                        .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Address.Region))
                        .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                        .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                        .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                        .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                        .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));
        }
    }
}

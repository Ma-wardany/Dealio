using Dealio.Core.DTOs.ApplicationUser;
using Dealio.Core.Features.ApplicationUser.Commands.Models;
using Dealio.Domain.Entities;

namespace Dealio.Core.Mappings.ApplicationUserMappings
{
    public partial class UserMappingsProfile
    {
        public void RegisterCommandsProfile()
        {
            CreateMap<RegisterCommand, ApplicationUser>()
                .ForMember(dest => dest.Email,    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ReverseMap();

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.UserId,   opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email,    opt => opt.MapFrom(src => src.Email))
                .ReverseMap();
        }
    }
}

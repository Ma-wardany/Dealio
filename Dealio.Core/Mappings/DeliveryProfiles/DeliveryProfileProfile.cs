using AutoMapper;
using Dealio.Core.DTOs.DeliveryProfile;
using Dealio.Core.Features.DeliveryProfile.Commands.Models;
using Dealio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Mappings.DeliveryProfiles
{
    public partial class DeliveryProfileProfile : Profile
    {
        public DeliveryProfileProfile()
        {
            CreateMap<DeliveryProfile, DeliveryProfileDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Address.Region))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));

                
            CreateDeliveryProfileMappings();
        }
    }
}

using AutoMapper;
using Dealio.Core.DTOs.Ratings;
using Dealio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Mappings.Ratings
{
    public partial class RatingProfile : Profile
    {
        public RatingProfile()
        {
            RatingCommandMappings();

            CreateMap<Rating, RatingDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.ApplicationUser.UserName))
                .ReverseMap();
        }
    }
}

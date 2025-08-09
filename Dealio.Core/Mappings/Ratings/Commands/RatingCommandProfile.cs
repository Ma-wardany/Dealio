using AutoMapper;
using Dealio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Mappings.Ratings
{
    public partial class RatingProfile
    {
        public void RatingCommandMappings()
        {
            CreateMap<Features.Ratings.Commands.Models.AddRatingCommand, Rating>();


            CreateMap<Features.Ratings.Commands.Models.UpdateRatingCommand, Rating>();
        }
    }
}

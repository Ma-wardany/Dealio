using Dealio.Core.Bases;
using Dealio.Core.DTOs.Ratings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Ratings.Commands.Models
{
    public class UpdateRatingCommand : IRequest<Response<RatingDto>>
    {
        public string Comment { get; set; }
        public int RatingValue { get; set; }
        public string UserId { get; set; }
    }
}

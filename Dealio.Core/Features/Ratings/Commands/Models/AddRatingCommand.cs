using Dealio.Core.Bases;
using Dealio.Core.DTOs.Ratings;
using MediatR;

namespace Dealio.Core.Features.Ratings.Commands.Models
{
    public class AddRatingCommand : IRequest<Response<RatingDto>>
    {
        public string Comment { get; set; }
        public int RatingValue { get; set; }
        public string UserId { get; set; }
    }
}


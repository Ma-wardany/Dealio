using Dealio.Core.Bases;
using Dealio.Core.DTOs.Ratings;
using Dealio.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Ratings.Queries.Models
{
    public class GetAllRatingQuery : IRequest<Response<List<RatingDto>>>
    {
    }
}

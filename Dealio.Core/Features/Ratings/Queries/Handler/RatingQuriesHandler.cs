using AutoMapper;
using Dealio.Core.Bases;
using Dealio.Core.DTOs.Ratings;
using Dealio.Core.Features.Ratings.Queries.Models;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Ratings.Queries.Handler
{
    public class RatingQuriesHandler : IRequestHandler<GetAllRatingQuery, Response<List<RatingDto>>>,
                                         IRequestHandler<GetRatingByUserQuery, Response<RatingDto>>
    {
        private readonly IMapper mapper;
        private readonly IRatingServices ratingServices;

        public RatingQuriesHandler(IMapper mapper, IRatingServices ratingServices)
        {
            this.mapper = mapper;
            this.ratingServices = ratingServices;
        }
        public async Task<Response<List<RatingDto>>> Handle(GetAllRatingQuery request, CancellationToken cancellationToken)
        {
            var result = await ratingServices.GetAllRatingsAsync();
            var status = result.ResultEnum;
            var ratings = result.Data;

            if (ratings.Any())
            {
                var ratingsDto = await mapper.ProjectTo<RatingDto>(ratings).ToListAsync();
                return Response<List<RatingDto>>.Success(ratingsDto);
            }

            return status switch
            {
                ServiceResultEnum.NotFound => Response<List<RatingDto>>.NotFound("No ratings found!"),
                ServiceResultEnum.Failed => Response<List<RatingDto>>.BadRequest("Something went wrong!"),
                _ => Response<List<RatingDto>>.BadRequest("Unknown error!")
            };
        }
        public async Task<Response<RatingDto>> Handle(GetRatingByUserQuery request, CancellationToken cancellationToken)
        {
            var result = await ratingServices.GetUserRatingAsync(request.UserId);
            var status = result.ResultEnum;
            var rating = result.Data;

            var ratingDto = mapper.Map<RatingDto>(rating);
            return status switch
            {
                ServiceResultEnum.Success => Response<RatingDto>.Success(ratingDto),
                ServiceResultEnum.NotFound => Response<RatingDto>.NotFound("No rating found for this user!"),
                ServiceResultEnum.Failed => Response<RatingDto>.BadRequest("Something went wrong!"),
                _ => Response<RatingDto>.BadRequest("Unknown error!")
            };
        }
    }
}

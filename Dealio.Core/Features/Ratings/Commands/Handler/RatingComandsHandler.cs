using AutoMapper;
using Dealio.Core.Bases;
using Dealio.Core.DTOs.Ratings;
using Dealio.Core.Features.Ratings.Commands.Models;
using Dealio.Domain.Entities;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Ratings.Commands.Handler
{
    public class RatingComandsHandler : IRequestHandler<AddRatingCommand, Response<RatingDto>>,
                                        IRequestHandler<UpdateRatingCommand, Response<RatingDto>>,
                                        IRequestHandler<DeleteRatingCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IRatingServices ratingServices;

        public RatingComandsHandler(IMapper mapper, IRatingServices ratingServices)
        {
            this.mapper = mapper;
            this.ratingServices = ratingServices;
        }

        public async Task<Response<RatingDto>> Handle(AddRatingCommand request, CancellationToken cancellationToken)
        {
            var ratingEntity = mapper.Map<Rating>(request);
            var result = await ratingServices.AddRatingAsync(ratingEntity);

            var status = result.ResultEnum;

            if (status == ServiceResultEnum.Created)
            {
                var ratingDto = mapper.Map<RatingDto>(result.Data);
                return Response<RatingDto>.Created(ratingDto);
            }

            return status switch
            {
                ServiceResultEnum.Already_Exist => Response<RatingDto>.BadRequest("already rated!"),
                ServiceResultEnum.Failed => Response<RatingDto>.BadRequest("something went wrong!"),
                _ => Response<RatingDto>.BadRequest("unknown error!")
            };
        }

        public async Task<Response<RatingDto>> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
        {
            var rating = mapper.Map<Rating>(request);
            var result = await ratingServices.UpdateRatingAsync(rating);

            var status = result.ResultEnum;

            if (status == ServiceResultEnum.Updated)
            {
                var ratingDto = mapper.Map<RatingDto>(result.Data);
                return Response<RatingDto>.Success(ratingDto);
            }

            return status switch
            {
                ServiceResultEnum.NotFound => Response<RatingDto>.BadRequest("no rating related ro this user"),
                ServiceResultEnum.Failed => Response<RatingDto>.BadRequest("something went wrong!"),
                _ => Response<RatingDto>.BadRequest("unknown error!")
            };
        }

        public async Task<Response<string>> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
        {
            var result = await ratingServices.DeleteRatingAsync(request.UserId);

            return result switch
            {
                ServiceResultEnum.Deleted => Response<string>.Success("ratind has been deleted successfully!"),
                ServiceResultEnum.NotFound => Response<string>.BadRequest("ratind is not found!"),
                ServiceResultEnum.Failed => Response<string>.BadRequest("something went wrong!"),
                _ => Response<string>.BadRequest("unknown error!")
            };
        }
    }
}

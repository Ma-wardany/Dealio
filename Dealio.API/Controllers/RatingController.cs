using Dealio.API.Base;
using Dealio.API.DTOs.Rating;
using Dealio.Core.Features.Ratings.Commands.Models;
using Dealio.Core.Features.Ratings.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Dealio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : AppController
    {

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddRatnig([FromForm] RatingRequestDto rating)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("unauthorized user");

            var command = new AddRatingCommand
            {
                UserId = userId,
                Comment = rating.Comment,
                RatingValue = rating.RatingValue
            };

            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRating([FromForm] RatingRequestDto rating)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("unauthorized user");
            var command = new AddRatingCommand
            {
                UserId = userId,
                Comment = rating.Comment,
                RatingValue = rating.RatingValue
            };
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteRating()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("unauthorized user");
            var command = new DeleteRatingCommand
            {
                UserId = userId
            };
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [Authorize]
        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetRating()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("unauthorized user");

            var query = new GetRatingByUserQuery
            {
                UserId = userId
            };
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllRatings()
        {
            var query = new GetAllRatingQuery();
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }
    }
}

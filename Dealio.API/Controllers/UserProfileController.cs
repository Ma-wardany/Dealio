using Dealio.API.DTOs.UserProfile;
using Dealio.Core.Features.UserProfile.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dealio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator mediator;

        public UserProfileController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserProfile([FromForm] ProfileRequestDto profile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Unauthorized user");

            var command = new AddUserProfileCommand
            {
                Model = new ProfileCommandModel
                {
                    UserId = userId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Phone = profile.Phone,
                    City = profile.City,
                    Region = profile.Region,
                    Street = profile.Street,
                    Image = profile.Image,
                }
            };

            var response = await mediator.Send(command);
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] ProfileRequestDto profile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Unauthorized user");
            var command = new UpdateUserProfileCommand
            {
                Model = new ProfileCommandModel
                {
                    UserId = userId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Phone = profile.Phone,
                    City = profile.City,
                    Region = profile.Region,
                    Street = profile.Street,
                    Image = profile.Image,
                }
            };
            var response = await mediator.Send(command);
            return Ok(response);
        }

    }
}

using Dealio.Core.Bases;
using Dealio.Core.DTOs.UserProfileDto;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dealio.Core.Features.UserProfile.Commands.Models
{
    public class AddUserProfileCommand : IRequest<Response<UserProfileDto>>
    {
        public ProfileCommandModel Model { get; set; }
    }
}

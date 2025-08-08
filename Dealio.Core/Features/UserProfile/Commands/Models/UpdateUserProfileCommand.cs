using Dealio.Core.Bases;
using Dealio.Core.DTOs.UserProfileDto;
using MediatR;

namespace Dealio.Core.Features.UserProfile.Commands.Models
{
    public class UpdateUserProfileCommand : IRequest<Response<UserProfileDto>>
    {
        public ProfileCommandModel Model { get; set; }

    }
}

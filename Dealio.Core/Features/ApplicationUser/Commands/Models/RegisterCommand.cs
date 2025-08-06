using Dealio.Core.Bases;
using Dealio.Core.DTOs.ApplicationUser;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace Dealio.Core.Features.ApplicationUser.Commands.Models
{
    public class RegisterCommand : IRequest<Response<UserDto>>
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

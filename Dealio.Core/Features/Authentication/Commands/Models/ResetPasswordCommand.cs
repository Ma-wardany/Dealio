using Dealio.Core.Bases;
using MediatR;

namespace Dealio.Core.Features.Authentication.Commands.Models
{
    public class ResetPasswordCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}



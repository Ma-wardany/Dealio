using Dealio.Core.Bases;
using Dealio.Domain.Results;
using MediatR;

namespace Dealio.Core.Features.Authentication.Commands.Models
{
    public class LoginCommand : IRequest<Response<AuthenticationResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

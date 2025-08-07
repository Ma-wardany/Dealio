using Dealio.Core.Bases;
using Dealio.Services.Helpers;
using MediatR;

namespace Dealio.Core.Features.Authentication.Commands.Models
{
    public class ForgetPasswordCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
    }
}

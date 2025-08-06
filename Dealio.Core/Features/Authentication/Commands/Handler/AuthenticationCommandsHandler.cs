using Dealio.Core.Bases;
using Dealio.Core.Features.Authentication.Commands.Models;
using Dealio.Domain.Results;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MediatR;

namespace Dealio.Core.Features.Authentication.Commands.Handler
{
    public class AuthenticationCommandsHandler : IRequestHandler<LoginCommand, Response<AuthenticationResult>>
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationCommandsHandler(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public async Task<Response<AuthenticationResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginResult = await authenticationService.LoginAsync(request.Email, request.Password);

            var status = loginResult.ResultEnum;
            var response = loginResult.Data;

            return status switch
            {
                ServiceResultEnum.Success => Response<AuthenticationResult>.Success(response, "logged in successfully"),
                ServiceResultEnum.NotFound => Response<AuthenticationResult>.NotFound("account not found"),
                ServiceResultEnum.NotConfirmed => Response<AuthenticationResult>.BadRequest("account not confirmed"),
                ServiceResultEnum.LockedOut => Response<AuthenticationResult>.BadRequest("try again later"),
                ServiceResultEnum.IncorrectEmailOrPassword => Response<AuthenticationResult>.BadRequest("invalid email or password"),
                _ => Response<AuthenticationResult>.BadRequest("invalid email or password")

            };

        }
    }
}

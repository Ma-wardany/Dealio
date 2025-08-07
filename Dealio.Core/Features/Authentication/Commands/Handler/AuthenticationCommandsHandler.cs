using Dealio.Core.Bases;
using Dealio.Core.Features.Authentication.Commands.Models;
using Dealio.Domain.Results;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MediatR;

namespace Dealio.Core.Features.Authentication.Commands.Handler
{
    public class AuthenticationCommandsHandler : IRequestHandler<LoginCommand, Response<AuthenticationResult>>,
                                                 IRequestHandler<ForgetPasswordCommand, Response<string>>,
                                                 IRequestHandler<ResetPasswordCommand, Response<string>>
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

        public async Task<Response<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await authenticationService.ForgetPassowrdHandler(request.Email);

            return result switch
            {
                ServiceResultEnum.Success => Response<string>.Success("Password reset email sent successfully"),
                ServiceResultEnum.NotFound => Response<string>.NotFound("Email not found"),
                ServiceResultEnum.NotConfirmed => Response<string>.BadRequest("Email not confirmed"),
                _ or ServiceResultEnum.Failed => Response<string>.BadRequest("An error occurred while processing your request")
            };
        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await authenticationService.ResetPasswordHandler(request.Email, request.Token, request.NewPassword);

            return result switch
            {
                ServiceResultEnum.Success => Response<string>.Success("Password reset successfully"),
                ServiceResultEnum.NotFound => Response<string>.NotFound("Email not found"),
                ServiceResultEnum.NotConfirmed => Response<string>.BadRequest("Email not confirmed"),
                _ or ServiceResultEnum.Failed => Response<string>.BadRequest("An error occurred while processing your request")
            };
        }
    }
}

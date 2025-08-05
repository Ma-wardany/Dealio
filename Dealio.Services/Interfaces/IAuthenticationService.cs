

using Dealio.Domain.Results;
using Dealio.Services.Helpers;

namespace Dealio.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<ServiceResult<AuthenticationResult>> LoginAsync(string email, string password);
        public Task<ServiceResultEnum> ForgetPassowrdHandler(string Email);
        public Task<ServiceResultEnum> ResetPasswordHandler(string email, string token, string newPassword);

    }
}

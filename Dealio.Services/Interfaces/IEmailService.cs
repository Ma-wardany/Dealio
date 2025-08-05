

using Dealio.Services.Helpers;
using Dealio.Services.ServicesImp;
using Dealio.Domain.Helpers;
namespace Dealio.Services.Interfaces
{
    public interface IEmailService
    {
        public Task<ServiceResultEnum> SendEmailAsync(EmailModel emailModel);
    }
}

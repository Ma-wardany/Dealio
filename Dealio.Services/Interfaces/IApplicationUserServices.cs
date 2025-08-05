
using Dealio.Domain.Entities;
using Dealio.Services.Helpers;

namespace Dealio.Services.Interfaces
{
    public interface IApplicationUserServices
    {
        public Task<ServiceResult<ApplicationUser>> Register(ApplicationUser applicationUser, string Password);
    }
}

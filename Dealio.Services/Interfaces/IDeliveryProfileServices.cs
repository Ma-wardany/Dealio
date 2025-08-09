using Dealio.Domain.Entities;
using Dealio.Services.Helpers;

namespace Dealio.Services.Interfaces
{
    public interface IDeliveryProfileServices
    {
        public Task<ServiceResult<DeliveryProfile>> CreateDeliveryProfileAsync(ApplicationUser user);
    }
}

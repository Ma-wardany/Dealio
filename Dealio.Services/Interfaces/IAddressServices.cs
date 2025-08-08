using Dealio.Domain.Entities;
using Dealio.Services.Helpers;

namespace Dealio.Services.Interfaces
{
    public interface IAddressServices
    {
        public Task<ServiceResultEnum> SetAddress(Address address);
    }
}

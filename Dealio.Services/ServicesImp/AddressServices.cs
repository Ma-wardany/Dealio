using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;

namespace Dealio.Services.ServicesImp
{
    public class AddressServices : IAddressServices
    {
        private readonly IAddressRepository addressRepository;

        public AddressServices(IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
        }
    }
}

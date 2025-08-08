using Dealio.Domain.Entities;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dealio.Services.ServicesImp
{
    public class AddressServices : IAddressServices
    {
        private readonly IAddressRepository addressRepository;
        private readonly IGeoLocationService geoLocationService;

        public AddressServices(IAddressRepository addressRepository, IGeoLocationService geoLocationService)
        {
            this.addressRepository = addressRepository;
            this.geoLocationService = geoLocationService;
        }

        public async Task<ServiceResult<Address>> GetAddressByUser(string userId)
        {
            var address = await addressRepository.GetTableNoTracking()
                                       .FirstOrDefaultAsync(a => a.UserId == userId);

            if (address == null)
                return ServiceResult<Address>.Failure(ServiceResultEnum.NotFound);

            return ServiceResult<Address>.Success(address, ServiceResultEnum.Success);
        }


        public async Task<ServiceResultEnum> SetAddress(Address address)
        {
            Console.WriteLine($"-----------------{address.City}--------------------");

            var existingAddress = await addressRepository.GetTableNoTracking()
                                 .FirstOrDefaultAsync(a => a.UserId == address.UserId);

            try
            {
                if(existingAddress == null)
                {
                    var (lat, lng) = await geoLocationService.GetCoordinatesAsync(address);
                    address.Latitude = lat;
                    address.Longitude = lng;

                    await addressRepository.AddAsync(address);
                    return ServiceResultEnum.Success;
                }
                else
                {
                    var (lat, lng) = await geoLocationService.GetCoordinatesAsync(existingAddress);
                    
                    existingAddress.City = address.City;
                    existingAddress.Street = address.Street;
                    existingAddress.Region = address.Region;
                    existingAddress.Latitude = lat;
                    existingAddress.Longitude = lng;

                    await addressRepository.UpdateAsync(existingAddress);
                    return ServiceResultEnum.Updated;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "---------------------------------");
                return ServiceResultEnum.Failed;
            }
        }
    }
}

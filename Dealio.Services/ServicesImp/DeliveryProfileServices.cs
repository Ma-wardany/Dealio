
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;

namespace Dealio.Services.ServicesImp
{
    public class DeliveryProfileServices : IDeliveryProfileServices
    {
        private readonly IDeliveryProfileRepository deliveryProfileRepository;

        public DeliveryProfileServices(IDeliveryProfileRepository deliveryProfileRepository)
        {
            this.deliveryProfileRepository = deliveryProfileRepository;
        }
    }
}

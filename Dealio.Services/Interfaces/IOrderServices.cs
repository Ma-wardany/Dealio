using Dealio.Domain.Entities;
using Dealio.Services.Helpers;

namespace Dealio.Services.Interfaces
{
    public interface IOrderServices
    {
        public Task<ServiceResult<Order>> CreateOrder(Order order);
    }
}

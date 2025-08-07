using Dealio.Domain.Entities;
using Dealio.Services.Helpers;

namespace Dealio.Services.Interfaces
{
    public interface IOrderServices
    {
        public Task<ServiceResult<Order>> CreateOrder(Order order);
        public Task<ServiceResult<string>> DeleteOrder(int orderId, string userId);
        public Task<ServiceResult<IQueryable<Order>>> GetBuyerOrders(string buyerId);
    }
}

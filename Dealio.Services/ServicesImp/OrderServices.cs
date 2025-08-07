using Dealio.Domain.Entities;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dealio.Services.ServicesImp
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;

        public OrderServices(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public async Task<ServiceResult<Order>> CreateOrder(Order order)
        {
            var product = await productRepository.GetByIdAsync(order.ProductId);
            if (product == null)
                return ServiceResult<Order>.Failure(ServiceResultEnum.NotFound);

            if (product.SellerId == order.BuyerId)
                return ServiceResult<Order>.Failure(ServiceResultEnum.SameSellerAndBuyer);

            try
            {
                await orderRepository.AddAsync(order);
                return ServiceResult<Order>.Success(order, ServiceResultEnum.Created);
            }
            catch (Exception ex)
            {
                return ServiceResult<Order>.Failure(ServiceResultEnum.Failed);
            }
        }

        public async Task<ServiceResult<string>> DeleteOrder(int orderId, string userId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return ServiceResult<string>.Failure(ServiceResultEnum.NotFound);

            if (order.BuyerId != userId)
                return ServiceResult<string>.Failure(ServiceResultEnum.NoAccess);

            try
            {
                await orderRepository.DeleteAsync(order);
                return ServiceResult<string>.Success("Order deleted successfully", ServiceResultEnum.Deleted);
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure(ServiceResultEnum.Failed);
            }
        }

        public async Task<ServiceResult<IQueryable<Order>>> GetBuyerOrders(string buyerId)
        {
            var orders = orderRepository.GetTableNoTracking()
                                        .Include(o => o.Product)
                                        .Where(o => o.BuyerId == buyerId);
            if (!orders.Any())
                return ServiceResult<IQueryable<Order>>.Failure(ServiceResultEnum.Empty);
            return ServiceResult<IQueryable<Order>>.Success(orders, ServiceResultEnum.Success);
        }
    }
}
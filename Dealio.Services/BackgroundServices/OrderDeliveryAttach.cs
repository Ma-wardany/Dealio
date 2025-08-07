using Dealio.Domain.Entities;
using Dealio.Domain.Enums;
using Dealio.Domain.Helpers;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dealio.Services.BackgroundServices
{
    public class OrderDeliveryAttach : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public OrderDeliveryAttach(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var deliveryRepo = scope.ServiceProvider.GetRequiredService<IDeliveryProfileRepository>();
                var userRepo = scope.ServiceProvider.GetRequiredService<IUserProfileRepository>();
                var geoService = scope.ServiceProvider.GetRequiredService<IGeoLocationService>();

                await AssignOrdersAsync(orderRepo, deliveryRepo, userRepo, geoService);

                await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            }
        }

        private async Task AssignOrdersAsync(
            IOrderRepository orderRepo,
            IDeliveryProfileRepository deliveryRepo,
            IUserProfileRepository userRepo,
            IGeoLocationService geoService)
        {
            var pendingOrders = await orderRepo.GetTableAsTracking()
                .Include(o => o.Product)
                    .ThenInclude(p => p.Seller)
                        .ThenInclude(s => s.Address)
                .Include(o => o.Buyer)
                    .ThenInclude(b => b.Address)
                .Where(o => o.DeliveryId == null && o.OrderStatus == OrderStatus.Pending)
                .ToListAsync();

            var deliveries = await deliveryRepo.GetTableNoTracking()
                .Include(d => d.Address)
                .Include(d => d.Orders)
                .ToListAsync();

            foreach (var order in pendingOrders)
            {
                var seller = order.Product.Seller;
                var buyer = order.Buyer;

                var sellerCoords = await geoService.GetCoordinatesAsync(seller.Address);
                var buyerCoords  = await geoService.GetCoordinatesAsync(buyer.Address);

                DeliveryProfile? bestDelivery = null;
                double minScore = double.MaxValue;

                foreach (var delivery in deliveries)
                {
                    //if (delivery.Address.City?.Trim().ToLower() != seller.Address.City?.Trim().ToLower())
                    //    continue;

                    var deliveryCoords = await geoService.GetCoordinatesAsync(delivery.Address);

                    var toSeller = GeoHelper.CalculateDistance(deliveryCoords, sellerCoords);
                    var toBuyer = GeoHelper.CalculateDistance(sellerCoords, buyerCoords);

                    var loadPenalty = (delivery.Orders?.Count ?? 0) * 2;
                    var totalScore = toSeller + toBuyer + loadPenalty;

                    if (totalScore < minScore)
                    {
                        minScore = totalScore;
                        bestDelivery = delivery;
                    }
                }

                if (bestDelivery != null)
                {
                    order.DeliveryId = bestDelivery.UserId;
                    order.OrderStatus = OrderStatus.Processing;
                    await orderRepo.UpdateAsync(order);

                }
            }
        }
    }
}

using Dealio.Domain.Entities;
using Dealio.Domain.Enums;
using Dealio.Domain.Helpers;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dealio.Services.BackgroundServices
{
    public class OrderDeliveryAttach : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderDeliveryAttach> _logger;
        private const int DELAY_MINUTES = 3;
        private const int ERROR_DELAY_MINUTES = 1;

        public OrderDeliveryAttach(IServiceProvider serviceProvider, ILogger<OrderDeliveryAttach> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OrderDeliveryAttach background service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    var deliveryRepo = scope.ServiceProvider.GetRequiredService<IDeliveryProfileRepository>();
                    var userRepo = scope.ServiceProvider.GetRequiredService<IUserProfileRepository>();
                    var geoService = scope.ServiceProvider.GetRequiredService<IGeoLocationService>();

                    await AssignOrdersAsync(orderRepo, deliveryRepo, userRepo, geoService);

                    _logger.LogInformation("Order assignment cycle completed successfully");
                    await Task.Delay(TimeSpan.FromMinutes(DELAY_MINUTES), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("OrderDeliveryAttach background service is stopping");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in OrderDeliveryAttach background service");

                    // Wait shorter time before retry on error
                    try
                    {
                        await Task.Delay(TimeSpan.FromMinutes(ERROR_DELAY_MINUTES), stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }

            _logger.LogInformation("OrderDeliveryAttach background service stopped");
        }

        private async Task AssignOrdersAsync(
            IOrderRepository orderRepo,
            IDeliveryProfileRepository deliveryRepo,
            IUserProfileRepository userRepo,
            IGeoLocationService geoService)
        {
            var pendingOrders = await GetPendingOrdersAsync(orderRepo);
            if (!pendingOrders.Any())
            {
                _logger.LogDebug("No pending orders found");
                return;
            }

            var deliveries = await GetAvailableDeliveriesAsync(deliveryRepo);
            if (!deliveries.Any())
            {
                _logger.LogWarning("No delivery profiles available for assignment");
                return;
            }

            _logger.LogInformation($"Processing {pendingOrders.Count} pending orders with {deliveries.Count} available deliveries");

            int successfulAssignments = 0;
            int failedAssignments = 0;

            foreach (var order in pendingOrders)
            {
                try
                {
                    var assigned = await TryAssignOrderToDelivery(order, deliveries, geoService, orderRepo);
                    if (assigned)
                    {
                        successfulAssignments++;
                        _logger.LogInformation($"Order {order.Id} assigned successfully");
                    }
                    else
                    {
                        failedAssignments++;
                        _logger.LogWarning($"Could not assign order {order.Id}");
                    }
                }
                catch (Exception ex)
                {
                    failedAssignments++;
                    _logger.LogError(ex, $"Error processing order {order.Id}");
                }
            }

            _logger.LogInformation($"Assignment completed: {successfulAssignments} successful, {failedAssignments} failed");
        }

        private async Task<List<Order>> GetPendingOrdersAsync(IOrderRepository orderRepo)
        {
            try
            {
                return await orderRepo.GetTableAsTracking()
                    .Include(o => o.Product)
                        .ThenInclude(p => p.Seller)
                            .ThenInclude(s => s.Address)
                    .Include(o => o.Buyer)
                        .ThenInclude(b => b.Address)
                    .Where(o => o.DeliveryId == null && o.OrderStatus == OrderStatus.Pending)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pending orders");
                return new List<Order>();
            }
        }

        private async Task<List<DeliveryProfile>> GetAvailableDeliveriesAsync(IDeliveryProfileRepository deliveryRepo)
        {
            try
            {
                return await deliveryRepo.GetTableNoTracking()
                    .Include(d => d.Address)
                    .Include(d => d.Orders)
                    .Where(d => d.Address != null) // Only deliveries with valid addresses
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching delivery profiles");
                return new List<DeliveryProfile>();
            }
        }

        private async Task<bool> TryAssignOrderToDelivery(
            Order order,
            List<DeliveryProfile> deliveries,
            IGeoLocationService geoService,
            IOrderRepository orderRepo)
        {
            try
            {
                // Validate order data
                if (order.Product?.Seller?.Address == null)
                {
                    _logger.LogWarning($"Order {order.Id} has invalid seller address");
                    return false;
                }

                if (order.Buyer?.Address == null)
                {
                    _logger.LogWarning($"Order {order.Id} has invalid buyer address");
                    return false;
                }

                // Get coordinates with error handling
                var sellerCoords = await GetCoordinatesSafely(geoService, order.Product.Seller.Address, "seller");
                var buyerCoords = await GetCoordinatesSafely(geoService, order.Buyer.Address, "buyer");

                if (!sellerCoords.HasValue || !buyerCoords.HasValue)
                {
                    _logger.LogWarning($"Could not get coordinates for order {order.Id}");
                    return false;
                }

                // Find best delivery
                var bestDelivery = await FindBestDelivery(deliveries, sellerCoords.Value, buyerCoords.Value, geoService);

                if (bestDelivery != null)
                {
                    await AssignOrderToDelivery(order, bestDelivery, orderRepo);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error assigning order {order.Id}");
                return false;
            }
        }

        private async Task<(double lat, double lon)?> GetCoordinatesSafely(
            IGeoLocationService geoService,
            Address address,
            string addressType)
        {
            try
            {
                var coords = await geoService.GetCoordinatesAsync(address);
                return coords;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to get {addressType} coordinates: {FormatAddressForLogging(address)}");
                return null;
            }
        }

        private async Task<DeliveryProfile?> FindBestDelivery(
            List<DeliveryProfile> deliveries,
            (double lat, double lon) sellerCoords,
            (double lat, double lon) buyerCoords,
            IGeoLocationService geoService)
        {
            DeliveryProfile? bestDelivery = null;
            double minScore = double.MaxValue;

            foreach (var delivery in deliveries)
            {
                try
                {
                    if (delivery.Address == null)
                    {
                        _logger.LogWarning($"Delivery profile {delivery.UserId} has no address");
                        continue;
                    }

                    var deliveryCoords = await GetCoordinatesSafely(geoService, delivery.Address, "delivery");
                    if (!deliveryCoords.HasValue)
                        continue;

                    var toSeller = GeoHelper.CalculateDistance(deliveryCoords.Value, sellerCoords);
                    var toBuyer = GeoHelper.CalculateDistance(sellerCoords, buyerCoords);
                    var loadPenalty = (delivery.Orders?.Count ?? 0) * 2;
                    var totalScore = toSeller + toBuyer + loadPenalty;

                    if (totalScore < minScore)
                    {
                        minScore = totalScore;
                        bestDelivery = delivery;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Error evaluating delivery profile {delivery.UserId}");
                }
            }

            if (bestDelivery != null)
            {
                _logger.LogInformation($"Best delivery found: {bestDelivery.UserId} with score: {minScore:F2}");
            }

            return bestDelivery;
        }

        private async Task AssignOrderToDelivery(Order order, DeliveryProfile delivery, IOrderRepository orderRepo)
        {
            try
            {
                order.DeliveryId = delivery.UserId;
                order.OrderStatus = OrderStatus.Processing;
                await orderRepo.UpdateAsync(order);

                _logger.LogInformation($"Order {order.Id} assigned to delivery {delivery.UserId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating order {order.Id} assignment");
                throw;
            }
        }

        private bool IsValidAddress(Address address)
        {
            return !string.IsNullOrWhiteSpace(address.Street) ||
                   !string.IsNullOrWhiteSpace(address.City) ||
                   !string.IsNullOrWhiteSpace(address.Region);
        }

        private string FormatAddressForLogging(Address address)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(address.Street)) parts.Add($"Street: {address.Street}");
            if (!string.IsNullOrWhiteSpace(address.City)) parts.Add($"City: {address.City}");
            if (!string.IsNullOrWhiteSpace(address.Region)) parts.Add($"Region: {address.Region}");

            return parts.Any() ? string.Join(", ", parts) : "Empty Address";
        }

    }
}
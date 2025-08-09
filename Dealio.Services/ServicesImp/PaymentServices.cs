using Dealio.Domain.Entities;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;

namespace Dealio.Services.ServicesImp
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentServices(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<ServiceResult<Payment>> ProcessFakePaymentAsync(string buyerId,
    int orderId,
    string paymentMethod,
    string cardInfo)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(buyerId))
                    return ServiceResult<Payment>.Failure(ServiceResultEnum.Failed);

                if (orderId <= 0)
                    return ServiceResult<Payment>.Failure(ServiceResultEnum.Failed);

                if (string.IsNullOrEmpty(paymentMethod))
                    return ServiceResult<Payment>.Failure(ServiceResultEnum.Failed);

                // Simulate payment processing
                var payment = new Payment
                {
                    BuyerId = buyerId,
                    OrderId = orderId,
                    PaymentMethod = paymentMethod,
                    CardInfo = string.IsNullOrEmpty(cardInfo) ? "Simulated Card" : cardInfo,
                    PaymentStatus = SimulatePaymentProcessing()
                };

                // Save the payment to the database
                var createdPayment = await _paymentRepository.AddAsync(payment);

                return ServiceResult<Payment>.Success(createdPayment, ServiceResultEnum.Created);
            }
            catch (Exception)
            {
                return ServiceResult<Payment>.Failure(ServiceResultEnum.Failed);
            }
        }

        private string SimulatePaymentProcessing()
        {
            // Simulate payment success or failure (e.g., 80% success rate)
            var random = new Random();
            var success = random.NextDouble() < 0.8;

            return success ? "Success" : "Failed";
        }

    }
}

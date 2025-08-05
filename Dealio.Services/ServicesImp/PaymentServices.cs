using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;

namespace Dealio.Services.ServicesImp
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentRepository paymentRepository;

        public PaymentServices(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }
    }
}

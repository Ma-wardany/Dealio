using Dealio.Domain.Entities;
using Dealio.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Services.Interfaces
{
    public interface IPaymentServices
    {
        public Task<ServiceResult<Payment>> ProcessFakePaymentAsync(
    string buyerId,
    int orderId,
    string paymentMethod,
    string cardInfo);
    }
}

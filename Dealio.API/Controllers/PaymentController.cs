using Dealio.Services;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using Dealio.Services.ServicesImp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dealio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices paymentServices;

        public PaymentController(IPaymentServices paymentServices)
        {
            this.paymentServices = paymentServices;
        }

        [HttpPost("process-fake-payment")]
        public async Task<IActionResult> ProcessFakePayment(
        string buyerId,
        int orderId,
        string paymentMethod,
        string? cardInfo = null)
        {
            if (string.IsNullOrEmpty(buyerId) || orderId <= 0 || string.IsNullOrEmpty(paymentMethod))
            {
                return BadRequest(new { Message = "Invalid input parameters." });
            }

            var result = await paymentServices.ProcessFakePaymentAsync(buyerId, orderId, paymentMethod, cardInfo);

            return result.ResultEnum switch
            {
                ServiceResultEnum.Created => Ok(new
                {
                    result.Data.Id,
                    result.Data.OrderId,
                    result.Data.BuyerId,
                    result.Data.PaymentMethod,
                    result.Data.PaymentStatus,
                    result.Data.CardInfo
                }),
                ServiceResultEnum.Failed => BadRequest(new { Message = "Payment processing failed." }),
                _ => StatusCode(500, new { Message = "Unexpected error occurred." })
            };
        }

        /// <summary>
        /// Retrieves a payment by its ID.
        /// </summary>
        /// <param name="id">The ID of the payment to retrieve.</param>
        ///// <returns>The payment details or an error message.</returns>
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetPaymentById(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest(new { Message = "Invalid payment ID." });
        //    }

        //    var payment = await paymentServices.GetByIdAsync(id);
        //    if (payment == null)
        //    {
        //        return NotFound(new { Message = "Payment not found." });
        //    }

        //    return Ok(new
        //    {
        //        payment.Id,
        //        payment.OrderId,
        //        payment.BuyerId,
        //        payment.PaymentMethod,
        //        payment.PaymentStatus,
        //        payment.CardInfo
        //    });
        //}

        /// <summary>
        /// Retrieves all payments for a specific buyer.
        /// </summary>
        /// <param name="buyerId">The ID of the buyer.</param>
        /// <returns>A list of payments for the buyer.</returns>
        //[HttpGet("buyer/{buyerId}")]
        //public async Task<IActionResult> GetPaymentsByBuyer(string buyerId)
        //{
        //    if (string.IsNullOrEmpty(buyerId))
        //    {
        //        return BadRequest(new { Message = "Invalid buyer ID." });
        //    }

        //    var payments = await paymentServices.GetPaymentsByBuyerAsync(buyerId);
        //    return Ok(payments.Select(p => new
        //    {
        //        p.Id,
        //        p.OrderId,
        //        p.BuyerId,
        //        p.PaymentMethod,
        //        p.PaymentStatus,
        //        p.CardInfo
        //    }));
        //}

    }
}

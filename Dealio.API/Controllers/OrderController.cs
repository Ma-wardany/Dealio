using Dealio.Core.Features.Orders.Commands.Models;
using Dealio.Core.Features.Orders.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dealio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddOrder([FromForm] int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Unauthorized user");
            var command = new AddOrderCommand
            {
                BuyerId = userId,
                ProductId = productId
            };
            var response = await mediator.Send(command);
            return StatusCode((int)response.StatusCode, response);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteOrder([FromQuery] int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Unauthorized user");

            var command = new DeleteOrderCommand
            {
                OrderId = orderId,
                BuyerId = userId
            };
            var response = await mediator.Send(command);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpGet("buyer-orders")]
        public async Task<IActionResult> GetBuyerOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Unauthorized user");

            var query = new GetBuyerOrdersQuery { BuyerId = userId };
            var response = await mediator.Send(query);
            return Ok(response);
        }
    }
}

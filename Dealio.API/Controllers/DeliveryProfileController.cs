using Dealio.API.Base;
using Dealio.Core.Features.DeliveryProfile.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dealio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryProfileController : AppController
    {

        [HttpPost("create")]
        public async Task<IActionResult> CreateDeliveryProfile([FromForm] CreateDeliveryProfileCommand command)
        {
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }
    }
}

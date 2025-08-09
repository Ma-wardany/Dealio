using Dealio.Core.Features.DeliveryProfile.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dealio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryProfileController : ControllerBase
    {
        private readonly IMediator mediator;

        public DeliveryProfileController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateDeliveryProfile([FromForm] CreateDeliveryProfileCommand command)
        {
            var response = await mediator.Send(command);
            return Ok(response);
        }
    }
}

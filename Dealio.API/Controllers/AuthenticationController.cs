using Dealio.Core.Features.Authentication.Commands.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dealio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthenticationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginCommand command)
        {
            var response = await mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromForm] ForgetPasswordCommand command)
        {
            var response = await mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordCommand command)
        {
            var response = await mediator.Send(command);
            return Content("response success");
        }
    }
}

using Dealio.Core.Bases;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Dealio.API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        protected IMediator Mediator => HttpContext.RequestServices.GetService<IMediator>()!;
        public ObjectResult FinalResponse<T>(Response<T> response) where T : class
        {
            return response.StatusCode switch
            {
                HttpStatusCode.OK                  => new OkObjectResult(response),
                HttpStatusCode.Unauthorized        => new UnauthorizedObjectResult(response),
                HttpStatusCode.Created             => new CreatedResult(string.Empty, response),
                HttpStatusCode.BadRequest          => new BadRequestObjectResult(response),
                HttpStatusCode.NotFound            => new NotFoundObjectResult(response),
                HttpStatusCode.UnprocessableEntity => new UnprocessableEntityObjectResult(response),
                _                                  => new BadRequestObjectResult(response)
            };
        }
    }
}

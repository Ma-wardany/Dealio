using Dealio.API.DTOs.Product;
using Dealio.Core.Features.Product.Commands.Models;
using Dealio.Core.Features.Product.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dealio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] AddProductDto product)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("unauthorized user");

            var command = new AddProductCommand
            {
                SellerId = userId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Images = product.Images
                
            }
            var response = await mediator.Send(command);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int prductId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
                return Unauthorized("unauthorized user");

            var command = new DeleteProductCommand { ProductId = prductId, SellerId = userId };

            var response = await mediator.Send(command);
            return Ok(response);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto product)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("unauthorized user");

            var command = new UpdateProductCommand
            {
                SellerId = userId,
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
            var response = await mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await mediator.Send(new GetAllProductsQuery());
            return Ok(response);
        }

        [HttpGet("products-by-user")]
        public async Task<IActionResult> GetProductsByUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("unauthorized user");

            var query = new GetProductsByUserQuery { UserId = userId };
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("product-by-id")]
        public async Task<IActionResult> GetProductById([FromQuery]GetProductByIdQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("products-by-category")]
        public async Task<IActionResult> GetProductsByCategory([FromQuery] GetProductsByCategoryQuery query)
        {
            var response = await mediator.Send(query);
            return Ok(response);
        }
    }
}

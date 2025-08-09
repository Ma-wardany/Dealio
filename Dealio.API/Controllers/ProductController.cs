using Dealio.API.Base;
using Dealio.API.DTOs.Product;
using Dealio.Core.Features.Product.Commands.Models;
using Dealio.Core.Features.Product.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dealio.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : AppController
    {

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] AddProductDto product)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("unauthorized user");

            Console.WriteLine($"userId: {userId} ---------------------------------------------");
            var command = new AddProductCommand
            {
                SellerId = userId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Images = product.Images

            };
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int prductId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
                return Unauthorized("unauthorized user");

            var command = new DeleteProductCommand { ProductId = prductId, SellerId = userId };

            var response = await Mediator.Send(command);
            return FinalResponse(response);
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
            var response = await Mediator.Send(command);
            return FinalResponse(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await Mediator.Send(new GetAllProductsQuery());
            return FinalResponse(response);
        }

        [HttpGet("products-by-user")]
        public async Task<IActionResult> GetProductsByUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("unauthorized user");

            var query = new GetProductsByUserQuery { UserId = userId };
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }

        [HttpGet("product-by-id")]
        public async Task<IActionResult> GetProductById([FromQuery]GetProductByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }

        [HttpGet("products-by-category")]
        public async Task<IActionResult> GetProductsByCategory([FromQuery] GetProductsByCategoryQuery query)
        {
            var response = await Mediator.Send(query);
            return FinalResponse(response);
        }
    }
}

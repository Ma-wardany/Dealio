using Dealio.Core.Bases;
using Dealio.Core.DTOs.Product;
using MediatR;


namespace Dealio.Core.Features.Product.Commands.Models
{
    public class UpdateProductCommand : IRequest<Response<ProductDto>>
    {
        public string SellerId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}

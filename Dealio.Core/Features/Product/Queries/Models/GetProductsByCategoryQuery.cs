using Dealio.Core.Bases;
using Dealio.Core.DTOs.Product;
using MediatR;


namespace Dealio.Core.Features.Product.Queries.Models
{
    public class GetProductsByCategoryQuery : IRequest<Response<List<ProductDto>>>
    {
        public int CategoryId { get; set; }

    }
}

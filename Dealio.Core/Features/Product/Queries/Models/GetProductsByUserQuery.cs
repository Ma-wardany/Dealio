using Dealio.Core.Bases;
using Dealio.Core.DTOs.Product;
using MediatR;

namespace Dealio.Core.Features.Product.Queries.Models
{
    public class GetProductsByUserQuery : IRequest<Response<List<ProductDto>>>
    {
        public string UserId { get; set; }
    }
}

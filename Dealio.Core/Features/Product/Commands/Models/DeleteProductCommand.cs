

using Dealio.Core.Bases;
using MediatR;

namespace Dealio.Core.Features.Product.Commands.Models
{
    public class DeleteProductCommand : IRequest<Response<string>>
    {
        public string SellerId { get; set; }
        public int ProductId { get; set; }
    }
}

using Dealio.Core.Bases;
using Dealio.Core.DTOs.Orders;
using MediatR;

namespace Dealio.Core.Features.Orders.Queries.Models
{
    public class GetBuyerOrdersQuery : IRequest<Response<List<OrderDto>>>
    {
        public string BuyerId { get; set; }
    }
}

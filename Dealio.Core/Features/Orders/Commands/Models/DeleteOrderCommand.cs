using Dealio.Core.Bases;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Orders.Commands.Models
{
    public class DeleteOrderCommand : IRequest<Response<string>>
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
    }
}

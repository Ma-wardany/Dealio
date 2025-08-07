
using Dealio.Core.Bases;
using Dealio.Core.DTOs.Orders;
using Dealio.Domain.Entities;
using Dealio.Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Dealio.Core.Features.Orders.Commands.Models
{
    public class AddOrderCommand : IRequest<Response<OrderDto>>
    {
        [Required]
        public string BuyerId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}

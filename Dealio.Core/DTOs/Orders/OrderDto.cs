using Dealio.Domain.Entities;
using Dealio.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderNumber { get; set; }
        public int ProductId { get; set; }
        public string BuyerId { get; set; }
        public string? DeliveryId { get; set; }
    }
}

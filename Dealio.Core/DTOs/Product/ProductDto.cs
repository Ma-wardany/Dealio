using Dealio.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? Status { get; set; }
        public string SellerId { get; set; }
        public int CategoryId { get; set; }
        public List<string> Images { get; set; }
    }
}

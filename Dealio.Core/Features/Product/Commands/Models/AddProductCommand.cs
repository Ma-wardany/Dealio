using Dealio.Core.Bases;
using Dealio.Core.DTOs.Product;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Product.Commands.Models
{
    public class AddProductCommand : IRequest<Response<ProductDto>>
    {
        public string Name        { get; set; }
        public string Description { get; set; }
        public string SellerId    { get; set; }
        public int CategoryId     { get; set; }
        public decimal Price      { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}

using Dealio.Core.Bases;
using Dealio.Core.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Product.Queries.Models
{
    public class GetProductByIdQuery : IRequest<Response<ProductDto>>
    {
        public int Id { get; set; }
    }
}

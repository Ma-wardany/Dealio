using Dealio.Core.Bases;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.Ratings.Commands.Models
{
    public class DeleteRatingCommand : IRequest<Response<string>>
    {
        public string UserId { get; set; }
    }
}

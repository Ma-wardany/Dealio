using Dealio.Core.Bases;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.ApplicationUser.Commands.Models
{
    public class ConfirmEmailCommand : IRequest<Response<string>>
    {
        public string UserId { get; set; }
        public string Code   { get; set; }
    }
}

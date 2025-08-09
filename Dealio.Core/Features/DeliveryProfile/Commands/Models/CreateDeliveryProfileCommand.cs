using Dealio.Core.Bases;
using Dealio.Core.DTOs.DeliveryProfile;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Core.Features.DeliveryProfile.Commands.Models
{
    public class CreateDeliveryProfileCommand : IRequest<Response<DeliveryProfileDto>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public IFormFile Image { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
    }
}

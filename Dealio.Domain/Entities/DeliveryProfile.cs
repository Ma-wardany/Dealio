using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Domain.Entities
{
    public class DeliveryProfile
    {
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public string Image { get; set; }
        public string Phone {  get; set; }

        public List<Order> Orders { get; set; }
        public Address Address { get; set; }
    }
}

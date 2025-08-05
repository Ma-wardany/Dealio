using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Domain.Entities
{
    public class UserProfile
    {
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Image { get; set; }
        public string Phone { get; set; }
        public List<Product> Products { get; set; }
        public List<Order> orders { get; set; }
        public List<SellerTransaction> SellerTransactions { get; set; }
        public List<Payment> Payments { get; set; }
        public Rating Rating { get; set; }
        public Address Address { get; set; }
    }
}

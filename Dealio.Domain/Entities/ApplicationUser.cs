
using Microsoft.AspNetCore.Identity;

namespace Dealio.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Address Address { get; set; }
        public UserProfile UserProfile { get; set; }
        public DeliveryProfile DeliveryProfile { get; set; }
    }
}

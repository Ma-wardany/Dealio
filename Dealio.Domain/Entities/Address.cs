using System.ComponentModel.DataAnnotations;

namespace Dealio.Domain.Entities
{
    public class Address
    {
        public int AddressId { get; set; }

        public string City { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}

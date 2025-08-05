using System.ComponentModel.DataAnnotations;

namespace Dealio.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string? Status { get; set; }

        
        public string SellerId { get; set; }
        public UserProfile Seller { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Order Order { get; set; }
        public List<ProductImage> Images { get; set; }

    }

}

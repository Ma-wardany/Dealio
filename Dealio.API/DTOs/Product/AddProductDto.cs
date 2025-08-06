namespace Dealio.API.DTOs.Product
{
    public class AddProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}

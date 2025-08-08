namespace Dealio.API.DTOs.UserProfile
{
    public class ProfileRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IFormFile? Image { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
    }
}

using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;

namespace Dealio.Services.ServicesImp
{
    public class RatingServices : IRatingServices
    {
        private readonly IRatingRepository ratingRepository;

        public RatingServices(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }
    }
}

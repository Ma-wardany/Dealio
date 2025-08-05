
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Interfaces;

namespace Dealio.Services.ServicesImp
{
    public class UserProfileServices : IUserProfileServices
    {
        private readonly IUserProfileRepository userProfileRepository;

        public UserProfileServices(IUserProfileRepository userProfileRepository)
        {
            this.userProfileRepository = userProfileRepository;
        }
    }
}

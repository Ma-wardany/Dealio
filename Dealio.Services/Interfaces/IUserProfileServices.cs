using Dealio.Domain.Entities;
using Dealio.Services.Helpers;


namespace Dealio.Services.Interfaces
{
    public interface IUserProfileServices
    {
        public Task<ServiceResult<UserProfile>> CreateUserProfile(UserProfile userProfile);
        public Task<ServiceResult<UserProfile>> UpdateUserProfile(UserProfile userProfile);
        public Task<ServiceResult<UserProfile>> GetUserProfile(string userId);
    }
}

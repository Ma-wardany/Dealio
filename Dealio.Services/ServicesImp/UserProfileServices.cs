
using Dealio.Domain.Entities;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dealio.Services.ServicesImp
{
    public class UserProfileServices : IUserProfileServices
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAddressServices addressServices;

        public UserProfileServices(
            IUserProfileRepository userProfileRepository,
            UserManager<ApplicationUser> userManager,
            IAddressServices addressServices)
        {
            this.userProfileRepository = userProfileRepository;
            this.userManager = userManager;
            this.addressServices = addressServices;
        }

        public async Task<ServiceResult<UserProfile>> CreateUserProfile(UserProfile userProfile)
        {
            var existingUser = await userManager.FindByIdAsync(userProfile.UserId);

            if (existingUser == null)
                return ServiceResult<UserProfile>.Failure(ServiceResultEnum.NotFound);

            if(existingUser.Id != userProfile.UserId)
                return ServiceResult<UserProfile>.Failure(ServiceResultEnum.NoAccess);

            try
            {
                userProfile.Address.UserId = existingUser.Id;
                await addressServices.SetAddress(userProfile.Address);

                await userProfileRepository.AddAsync(userProfile);

                return ServiceResult<UserProfile>.Success(userProfile, ServiceResultEnum.Created);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ServiceResult<UserProfile>.Failure(ServiceResultEnum.Failed);
            }
        }


        public async Task<ServiceResult<UserProfile>> GetUserProfile(string userId)
        {
            var existingUser = await userManager.FindByIdAsync(userId);

            if (existingUser == null)
                return ServiceResult<UserProfile>.Failure(ServiceResultEnum.NotFound);

            var userProfile = await userProfileRepository.GetTableNoTracking()
                                                         .Include(up => up.Address)
                                                         .SingleOrDefaultAsync(up => up.UserId == userId);

            return ServiceResult<UserProfile>.Success(userProfile, ServiceResultEnum.Success);
        }

        public async Task<ServiceResult<UserProfile>> UpdateUserProfile(UserProfile userProfile)
        {
            var existingUser = await userManager.FindByIdAsync(userProfile.UserId);
            if (existingUser == null)
                return ServiceResult<UserProfile>.Failure(ServiceResultEnum.NotFound);


            var profile = await userProfileRepository.GetTableAsTracking()
                                            .Include(up => up.Address)
                                            .FirstOrDefaultAsync(up => up.UserId == userProfile.UserId);

            if (profile == null)
                return ServiceResult<UserProfile>.Failure(ServiceResultEnum.NotFound);

            try
            {
                profile.FirstName = userProfile.FirstName;
                profile.LastName  = userProfile.LastName;
                profile.Phone = userProfile.Phone;

                if (profile.Address == null)
                {
                    profile.Address = new Address();
                }

                profile.Address.City = userProfile.Address.City;
                profile.Address.Street = userProfile.Address.Street;
                profile.Address.Region = userProfile.Address.Region;
                profile.Address.Latitude = userProfile.Address.Latitude;
                profile.Address.Longitude = userProfile.Address.Longitude;
                profile.Address.UserId = userProfile.UserId;

                await userProfileRepository.SaveChangesAsync();
                return ServiceResult<UserProfile>.Success(profile, ServiceResultEnum.Updated);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ServiceResult<UserProfile>.Failure(ServiceResultEnum.Failed);
            }
        }
    }
}

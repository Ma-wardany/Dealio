
using Dealio.Domain.Entities;
using Dealio.Infrastructure.DbContexts;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Infrastructure.Repositories.RepositoriesImp;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Dealio.Services.ServicesImp
{
    public class DeliveryProfileServices : IDeliveryProfileServices
    {
        private readonly IDeliveryProfileRepository deliveryProfileRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IAddressServices addressServices;

        public DeliveryProfileServices(
            IDeliveryProfileRepository deliveryProfileRepository,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IAddressServices addressServices)
        {
            this.deliveryProfileRepository = deliveryProfileRepository;
            this.userManager = userManager;
            this.context = context;
            this.addressServices = addressServices;
        }

        public async Task<ServiceResult<DeliveryProfile>> CreateDeliveryProfileAsync(ApplicationUser user)
        {
            var existingUser = await userManager.FindByIdAsync(user.Id);

            if (existingUser != null)
                return ServiceResult<DeliveryProfile>.Failure(ServiceResultEnum.UserAlreadyExists);

            var random = new Random();
            var randomNumber = random.Next(1000, 9999);

            // Create random username and email
            var baseName = user.DeliveryProfile.FirstName?.Trim().ToLower() ?? "user";
            user.UserName = $"{baseName}{randomNumber}";
            user.Email = $"{baseName}{randomNumber}@dealio.com";

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // 1️⃣ Insert Address first (only if provided)
                if (user.DeliveryProfile.Address != null)
                {
                    // This will insert the address and get coordinates
                    var addressResult = await addressServices.SetAddress(user.DeliveryProfile.Address);

                    if (addressResult == ServiceResultEnum.Failed)
                    {
                        await transaction.RollbackAsync();
                        return ServiceResult<DeliveryProfile>.Failure(ServiceResultEnum.Failed);
                    }

                    // Ensure AddressId is linked to DeliveryProfile
                    user.DeliveryProfile.AddressId = user.DeliveryProfile.Address.AddressId;
                }

                // 2️⃣ Create the user (which includes DeliveryProfile)
                var result = await userManager.CreateAsync(user, "Dealio@1234");
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<DeliveryProfile>.Failure(ServiceResultEnum.Failed);
                }

                await transaction.CommitAsync();

                return ServiceResult<DeliveryProfile>.Success(user.DeliveryProfile, ServiceResultEnum.Created);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                return ServiceResult<DeliveryProfile>.Failure(ServiceResultEnum.Failed);
            }
        }

    }
}

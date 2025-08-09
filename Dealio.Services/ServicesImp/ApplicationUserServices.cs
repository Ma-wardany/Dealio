using Dealio.Domain.Entities;
using Dealio.Domain.Helpers;
using Dealio.Infrastructure.DbContexts;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Dealio.Services.ServicesImp
{
    public class ApplicationUserServices : IApplicationUserServices
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUrlHelper urlHelper;
        private readonly IEmailService emailService;

        public ApplicationUserServices(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelper urlHelper,
            IEmailService emailService)
        {
            this.userManager = userManager;
            this.context     = context;
            this.httpContextAccessor = httpContextAccessor;
            this.urlHelper   = urlHelper;
            this.emailService = emailService;
        }

        public async Task<ServiceResult<ApplicationUser>> Register(ApplicationUser applicationUser, string password)
        {
            var existingUser = await userManager.FindByEmailAsync(applicationUser.Email!);
            if (existingUser != null)
                return ServiceResult<ApplicationUser>.Failure(ServiceResultEnum.UserAlreadyExists);

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var registerResult = await userManager.CreateAsync(applicationUser, password);
                if (!registerResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<ApplicationUser>.Failure(ServiceResultEnum.Failed);
                }

                var roleResult = await userManager.AddToRoleAsync(applicationUser, "user");
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<ApplicationUser>.Failure(ServiceResultEnum.Failed);
                }

                var code = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var request = httpContextAccessor.HttpContext!.Request;
                var returnUrl = request.Scheme + "://" + request.Host + urlHelper.Action("ConfirmEmail", "ApplicationUser", new { userId = applicationUser.Id, code = code });
                var message = $"To confirm your email, click this link: <a href='{returnUrl}'>Confirm Email</a>";


                var emailServiceResult = await emailService.SendEmailAsync(new EmailModel
                {
                    Email = applicationUser.Email!,
                    Subject = "Confirm your email",
                    Message = message
                });

                if (emailServiceResult != ServiceResultEnum.Success)
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<ApplicationUser>.Failure(ServiceResultEnum.Failed);
                }

                await transaction.CommitAsync();
                return ServiceResult<ApplicationUser>.Success(applicationUser, ServiceResultEnum.Created);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await transaction.RollbackAsync();
                return ServiceResult<ApplicationUser>.Failure(ServiceResultEnum.Failed);
            }
        }




        public async Task<ServiceResultEnum> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return ServiceResultEnum.NotFound;

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return ServiceResultEnum.NotFound;

            var result = await userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? ServiceResultEnum.Success : ServiceResultEnum.Failed;
        }

    }
}
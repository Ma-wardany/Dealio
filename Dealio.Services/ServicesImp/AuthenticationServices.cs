using Dealio.Domain.Entities;
using Dealio.Domain.Helpers;
using Dealio.Domain.Results;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Dealio.Services.ServicesImp
{
    public class AuthenticationServices : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUrlHelper urlHelper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEmailService emailService;
        private readonly JwtSettings jwtSettings;

        public AuthenticationServices(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtSettings,
            IUrlHelper urlHelper, 
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService)
        {
            this.userManager   = userManager;
            this.signInManager = signInManager;
            this.urlHelper = urlHelper;
            this.httpContextAccessor = httpContextAccessor;
            this.emailService = emailService;
            this.jwtSettings = jwtSettings.Value;
        }
        public async Task<ServiceResult<AuthenticationResult>> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return ServiceResult<AuthenticationResult>.Failure(ServiceResultEnum.NotFound);

            if(!user.EmailConfirmed)
                return ServiceResult<AuthenticationResult>.Failure(ServiceResultEnum.NotConfirmed);

            // signIn
            var result = await signInManager.PasswordSignInAsync(user, password, lockoutOnFailure: true, isPersistent: false);
            if(!result.Succeeded)
            {
                if(result.IsLockedOut)
                    return ServiceResult<AuthenticationResult>.Failure(ServiceResultEnum.LockedOut);
                return ServiceResult<AuthenticationResult>.Failure(ServiceResultEnum.IncorrectEmailOrPassword);
            }

            var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();

            var token = await GenerateAccessToken(user);
            var authenticationResult = new AuthenticationResult
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                AccessToken = token,
                IsConfirmed = user.EmailConfirmed,
                Role = role
            };

            return ServiceResult<AuthenticationResult>.Success(authenticationResult, ServiceResultEnum.Success);
        }


        public async Task<ServiceResultEnum> ForgetPassowrdHandler(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null)
                return ServiceResultEnum.NotFound;

            if (!user.EmailConfirmed)
                return ServiceResultEnum.NotConfirmed;

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var request = httpContextAccessor.HttpContext!.Request;
            var returnUrl = request.Scheme + "://" + request.Host + urlHelper.Action("ResetPassword", "Authentication", new { Email = user.Email, Token = token });

            var message = $"Please reset your password by clicking here: <a href='{returnUrl}'>Reset Password</a>";
            var emailServiceResult = await emailService.SendEmailAsync(new EmailModel
            {
                Email = user.Email,
                Subject = "Reset Password",
                Message = message
            });

            if (emailServiceResult != ServiceResultEnum.Success)
                return emailServiceResult;

            return ServiceResultEnum.Success;
        }

        public async Task<ServiceResultEnum> ResetPasswordHandler(string email, string token, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return ServiceResultEnum.NotFound;

            if (!user.EmailConfirmed)
                return ServiceResultEnum.NotConfirmed;

            var resetResult = await userManager.ResetPasswordAsync(user, token, newPassword);
            if (!resetResult.Succeeded)
                return ServiceResultEnum.Failed;

            return ServiceResultEnum.Success;
        }



        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var JwtToken = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            return token;
        }
    }
}

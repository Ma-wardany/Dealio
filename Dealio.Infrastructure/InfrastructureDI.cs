using Dealio.Domain.Entities;
using Dealio.Domain.Helpers;
using Dealio.Domain.Results;
using Dealio.Infrastructure.DbContexts;
using Dealio.Infrastructure.Repositories.Interfaces;
using Dealio.Infrastructure.Repositories.RepositoriesImp;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dealio.Infrastructure
{
    public static class InfrastructureDI
    {
        public static IServiceCollection InfrastructireDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<AuthenticationResult>(configuration.GetSection("JWTSettings"));


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICommissionRepository, CommissionRepository>();
            services.AddScoped<IDeliveryProfileRepository, DeliveryProfileRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<ISellerTransactionRepository, SellerTransactionRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            return services;
        }
    }
}

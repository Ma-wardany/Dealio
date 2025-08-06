using Dealio.Services.BackgroundServices;
using Dealio.Services.Interfaces;
using Dealio.Services.ServicesImp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;


namespace Dealio.Services
{
    public static class ServicesDI
    {
        public static IServiceCollection AddServiceDI(this IServiceCollection services)
        {
            services.AddScoped<IAddressServices,           AddressServices>();
            services.AddScoped<IApplicationUserServices,   ApplicationUserServices>();
            services.AddScoped<IAuthenticationService, AuthenticationServices>();
            services.AddScoped<ICategoryServices,          CategoryServices>();
            services.AddScoped<ICommissionServices,        CommissionServices>();
            services.AddScoped<IDeliveryProfileServices,   DeliveryProfileServices>();
            services.AddScoped<IOrderServices,             OrderServices>();
            services.AddScoped<IPaymentServices,           PaymentServices>();
            services.AddScoped<IProductServices,           ProductServices>();
            services.AddScoped<IRatingServices,            RatingServices>();
            services.AddScoped<ISellerTransactionServices, SellerTransactionServices>();
            services.AddScoped<IUserProfileServices,       UserProfileServices>();
            services.AddScoped<IEmailService, EmailService>();


            services.AddHttpClient<IGeoLocationService,    NominatimGeoLocationService>();
            services.AddHostedService<OrderDeliveryAttach>();

            services.AddHttpClient();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            return services;
        }
    }
}

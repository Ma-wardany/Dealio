
using Dealio.Core;
using Dealio.Infrastructure;
using Dealio.Services;
using Dealio.Services.BackgroundServices;
using Dealio.Services.Interfaces;
using Dealio.Services.ServicesImp;
using Microsoft.OpenApi.Models;
namespace Dealio.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dealio API", Version = "v1" });

                // Define the BearerAuth scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token like: **Bearer your_token_here**"
                });

                // Apply the Bearer scheme globally to all endpoints
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });


            builder.Services.AddHttpClient<IGeoLocationService, NominatimGeoLocationService>(client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent", "DealioApp/1.0 (contact@dealio.com)");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            // Configure Host options to prevent Background Service from stopping the app
            builder.Services.Configure<HostOptions>(options =>
            {
                options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
                options.ServicesStartConcurrently = true;
                options.ServicesStopConcurrently = true;
            });

            // Register the background service
            builder.Services.AddHostedService<OrderDeliveryAttach>();

            // Add logging configuration
            builder.Services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();

                if (builder.Environment.IsDevelopment())
                {
                    logging.SetMinimumLevel(LogLevel.Debug);
                }
                else
                {
                    logging.SetMinimumLevel(LogLevel.Information);
                }
            });


            builder.Services.InfrastructireDI(builder.Configuration)
                            .AddServiceDI()
                            .CoreDependencies();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000", "https://yourdomain.com") // حط الـ frontend URLs
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty; // يخلي Swagger UI يفتح على رابط الصفحة الرئيسية
                });
            }
            app.UseStaticFiles();
            // قبل app.Run()
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using AutoMapper;
using Dealio.Core.Bases;
using Dealio.Core.DTOs.DeliveryProfile;
using Dealio.Core.Features.DeliveryProfile.Commands.Models;
using Dealio.Domain.Entities;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Dealio.Core.Features.DeliveryProfile.Commands.Handler
{
    public class DeliveryProfileCommndsHandler : IRequestHandler<CreateDeliveryProfileCommand, Response<DeliveryProfileDto>>
    {
        private readonly IMapper mapper;
        private readonly IDeliveryProfileServices deliveryProfileServices;
        private readonly IGeoLocationService geoLocation;
        private readonly IWebHostEnvironment environment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DeliveryProfileCommndsHandler(IMapper mapper,
            IDeliveryProfileServices deliveryProfileServices,
            IGeoLocationService geoLocation,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.mapper = mapper;
            this.deliveryProfileServices = deliveryProfileServices;
            this.geoLocation = geoLocation;
            this.environment = environment;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<DeliveryProfileDto>> Handle(CreateDeliveryProfileCommand request, CancellationToken cancellationToken)
        {
            var user = mapper.Map<Domain.Entities.ApplicationUser>(request);

            // ✅ Handle image upload
            if (request.Image != null && request.Image.Length > 0)
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
                var imageFolder = Path.Combine(environment.WebRootPath, "Images");
                var imagePath = Path.Combine(imageFolder, imageName);

                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }

                var baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
                user.DeliveryProfile.Image = $"{baseUrl}/Images/{imageName}";
            }

            // ✅ المهم جداً: فصل الـ Address عن الـ DeliveryProfile
            user.DeliveryProfile.Address = null;

            var result = await deliveryProfileServices.CreateDeliveryProfileAsync(user);
            var status = result.ResultEnum;
            
            var deliveryProfile = mapper.Map<DeliveryProfileDto>(result.Data);

            return status switch
            {
                ServiceResultEnum.Created => Response<DeliveryProfileDto>.Created(deliveryProfile, "Delivery profile created successfully"),
                ServiceResultEnum.UserAlreadyExists => Response<DeliveryProfileDto>.BadRequest("User already exists"),
                ServiceResultEnum.NotFound => Response<DeliveryProfileDto>.NotFound("User not found"),
                _ or ServiceResultEnum.Failed => Response<DeliveryProfileDto>.BadRequest("Something went wrong!")
            };
        }
    }
}

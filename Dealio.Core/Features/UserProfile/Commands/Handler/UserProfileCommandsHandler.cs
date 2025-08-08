using AutoMapper;
using Dealio.Core.Bases;
using Dealio.Core.DTOs.UserProfileDto;
using Dealio.Core.Features.UserProfile.Commands.Models;
using Dealio.Domain.Entities;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;


namespace Dealio.Core.Features.UserProfile.Commands.Handler
{
    public class UserProfileCommandsHandler : IRequestHandler<AddUserProfileCommand, Response<UserProfileDto>>,
                                              IRequestHandler<UpdateUserProfileCommand, Response<UserProfileDto>>
    {
        private readonly IUserProfileServices userProfileServices;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment environment;
        private readonly IGeoLocationService geoLocation;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserProfileCommandsHandler(IUserProfileServices userProfileServices,
            IMapper mapper,
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            this.mapper              = mapper;
            this.environment = environment;
            this.httpContextAccessor = httpContextAccessor;
            this.userProfileServices = userProfileServices;
        }
        public async Task<Response<UserProfileDto>> Handle(AddUserProfileCommand request, CancellationToken cancellationToken)
        {
            // Map incoming data to entity
            var profile = mapper.Map<Domain.Entities.UserProfile>(request.Model);

            //-----------------------------------
            Console.WriteLine($"{profile.FirstName}-{profile.UserId}-{profile.Address.City}");

            // 🟡 Handle image upload
            if (request.Model.Image != null && request.Model.Image.Length > 0)
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(request.Model.Image.FileName)}";
                var imageFolder = Path.Combine(environment.WebRootPath, "Images");
                var imagePath = Path.Combine(imageFolder, imageName);

                // Ensure directory exists
                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.Model.Image.CopyToAsync(stream);
                }
                var baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
                profile.Image = $"{baseUrl}/Images/{imageName}";
            }


            // Call service to create profile
            var result = await userProfileServices.CreateUserProfile(profile);

            // Map result to DTO
            var userProfile = mapper.Map<UserProfileDto>(result.Data);

            // Return response
            return result.ResultEnum switch
            {
                ServiceResultEnum.Created => Response<UserProfileDto>.Success(userProfile, "User profile created successfully"),
                ServiceResultEnum.Failed => Response<UserProfileDto>.BadRequest("Failed to create user profile"),
                ServiceResultEnum.NoAccess => Response<UserProfileDto>.BadRequest("This profile doesn't belong to the user ID"),
                ServiceResultEnum.NotFound => Response<UserProfileDto>.NotFound("User not found"),
                _ => Response<UserProfileDto>.BadRequest("Unknown error")
            };
        }

        public async Task<Response<UserProfileDto>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = mapper.Map<Domain.Entities.UserProfile>(request.Model);
            var result = await userProfileServices.UpdateUserProfile(profile);

            var status = result.ResultEnum;
            var userProfile = mapper.Map<UserProfileDto>(result.Data);

            // 🟡 Handle image upload
            if (request.Model.Image != null && request.Model.Image.Length > 0)
            {
                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(request.Model.Image.FileName)}";
                var imageFolder = Path.Combine(environment.WebRootPath, "Images");
                var imagePath = Path.Combine(imageFolder, imageName);

                // Ensure directory exists
                if (!Directory.Exists(imageFolder))
                    Directory.CreateDirectory(imageFolder);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await request.Model.Image.CopyToAsync(stream);
                }
                var baseUrl = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
                profile.Image = $"{baseUrl}/Images/{imageName}";
            }

            return status switch
            {
                ServiceResultEnum.Updated => Response<UserProfileDto>.Success(userProfile, "User profile updated successfully"),
                ServiceResultEnum.Failed => Response<UserProfileDto>.BadRequest("Failed to create user profile"),
                ServiceResultEnum.NoAccess => Response<UserProfileDto>.BadRequest("this profile isn't belong to this user id"),
                ServiceResultEnum.NotFound => Response<UserProfileDto>.NotFound("User profile not found")
            };
        }
    }
}

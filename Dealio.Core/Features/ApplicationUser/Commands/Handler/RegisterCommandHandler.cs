using AutoMapper;
using Dealio.Core.Bases;
using Dealio.Core.DTOs.ApplicationUser;
using Dealio.Core.Features.ApplicationUser.Commands.Models;
using Dealio.Services.Helpers;
using Dealio.Services.Interfaces;
using MediatR;


namespace Dealio.Core.Features.ApplicationUser.Commands.Handler
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<UserDto>>
    {
        private readonly IMapper mapper;
        private readonly IApplicationUserServices applicationUserServices;

        public RegisterCommandHandler(IMapper mapper, IApplicationUserServices applicationUserServices)
        {
            this.mapper = mapper;
            this.applicationUserServices = applicationUserServices;
        }

        public async Task<Response<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var mappedUser = mapper.Map<Domain.Entities.ApplicationUser>(request);

            var result = await applicationUserServices.Register(mappedUser, request.Password);
            var user   = mapper.Map<UserDto>(result.Data);
            var status = result.ResultEnum;

            return status switch
            {
                ServiceResultEnum.Created           => Response<UserDto>.Created(user, "User created successfully"),
                ServiceResultEnum.UserAlreadyExists => Response<UserDto>.BadRequest("user is already exist"),
                ServiceResultEnum.NotFound          => Response<UserDto>.NotFound("user not found"),
                _ or ServiceResultEnum.Failed       => Response<UserDto>.BadRequest("something went wrong!")
            };
        }
    }
}

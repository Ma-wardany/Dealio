using AutoMapper;

namespace Dealio.Core.Mappings.ApplicationUserMappings
{
    public partial class UserMappingsProfile : Profile
    {
        public UserMappingsProfile()
        {
            RegisterCommandsProfile();
        }
    }
}
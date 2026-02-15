using AutoMapper;
using BWA.APIInfrastructure.Requests;
using BWA.ServiceEntities;

namespace BWA.APIInfrastructure.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ChangePasswordRequest, ChangePasswordDto>();
            CreateMap<DeactivateUserRequest, DeactivateUserDto>();
            CreateMap<ActivateUserRequest, ActivateUserDto>();
        }
    }
}

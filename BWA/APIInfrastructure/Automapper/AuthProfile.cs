using AutoMapper;
using BWA.APIInfrastructure.Requests;
using BWA.ServiceEntities;

namespace BWA.APIInfrastructure.Automapper
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<LoginRequest, LoginDto>();
        }
    }
}

using AutoMapper;
using BWA.APIInfrastructure.Attributes;
using BWA.APIInfrastructure.Requests;
using BWA.DomainEntities;
using BWA.ServiceEntities;
using BWA.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BWA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IMapper mapper
            , IHttpContextAccessor accessor, ICommonService commonService)
             : base(logger, mapper, accessor, commonService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("LoginAsync")]
        public async  Task<Dictionary<string, object>> LoginAsync(LoginRequest request)
        {
            var reqestDto = _mapper.Map<LoginDto>(request);
            var result = await _authService.LoginAsync(reqestDto);
            return APIResponse(result, "Success");

        }

        [BWAAuthorization]
        [HttpPost("LogoutAsync")]
        public async  Task<Dictionary<string, object>> LogoutAsync()
        {
            var result = await _authService.LogOutAsync(new LogOutDto()
            {
                ActionBy= SessionDetails.UserId
            });

            return APIResponse(result, "Success");
        }
    }
}

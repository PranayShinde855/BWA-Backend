using AutoMapper;
using BWA.APIInfrastructure.Attributes;
using BWA.APIInfrastructure.Requests;
using BWA.ServiceEntities;
using BWA.Services.interfaces;
using BWA.Utility.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BWA.Controllers
{
    [BWAAuthorization]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService, ILogger<UserController> logger, IMapper mapper
            , IHttpContextAccessor accessor, ICommonService commonService)
             : base(logger, mapper, accessor, commonService)
        {
            _userService = userService;
        }

        [HttpPost("GetAsync")]
        public async  Task<Dictionary<string, object>> GetAsync([FromBody] GetUsersRequest request)
        {
            var users = await _userService.GetUsersAsync(new GetUsersDto()
            {
                GlobalSearch = request.GlobalSearch,
                OrderBy = request.OrderBy,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            });
            //return Ok(users);
            return APIResponse(users, "Success");

        }

        [HttpPost("AddAsync")]
        public async  Task<Dictionary<string, object>> AddAsync([FromBody] AddUserRequest request)
        {
            var user = new AddUserDto
            {
                RoleId = request.RoleId,
                Salutation = request.Salutation,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                ISDCode = request.ISDCode,
                MobileNumber = request.MobileNumber,
                Image = request.Image,
                ImageExtension = request.ImageExtension,
                ImageName = request.ImageName
            };
            var result = await _userService.AddUserAsync(user);
            return APIResponse(user, "User saved successfully.");
        }

        [HttpPost("UpdateAsync")]
        public async  Task<Dictionary<string, object>> UpdateAsync([FromBody] UpdateUserRequest request)
        {
            var result = await _userService.UpdateUserAsync(new UpdateUserDto()
            {
                Id = request.Id,
                RoleId = request.RoleId,
                Salutation = request.Salutation,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                ISDCode = request.ISDCode,
                MobileNumber = request.MobileNumber,
                Image = request.Image,
                ImageName = request.ImageName,
                ImageExtension = request.ImageExtension,
                RemoveImage = request.RemoveImage
            });

            return APIResponse(result, "User updated successfully.");
        }

        [HttpPost("GetByIdAsync")]
        public async  Task<Dictionary<string, object>> GetByIdAsync([FromBody] GetUserByIdRequest request)
        {
            var user = await _userService.GetUserByIdAsync(new GetUserByIdDto()
            {
                Id = request.Id
            });

            //return Ok(user);
            return APIResponse(user, "Success");
        }

        [HttpPost("DeleteAsync")]
        public async  Task<Dictionary<string, object>> DeleteAsync([FromBody] DeleteUserRequest request)
        {
            var user = await _userService.DeleteUserAsync(new DeleteUserDto()
            {
                Id = request.Id
            });

            return APIResponse(user, "User deleted successfully.");
        }

        [HttpPost("GetIsdCodes")]
        public async  Task<Dictionary<string, object>> GetIsdCodes()
        {
            var user = await _userService.GetISDCodesAsync();
            return APIResponse(user, "Success");
        }

        [HttpPost("GetRolesAsync")]
        public async  Task<Dictionary<string, object>> GetRolesAsync()
        {
            var user = await _userService.GetRolesAsync();
            return APIResponse(user, "Success");
        }

        [HttpPost("ChangePasswordAsync")]
        public async Task<Dictionary<string, object>> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmPassword)
                throw new BadResultException("New password and confirm password should be smae.");

            var requestDto = _mapper.Map<ChangePasswordDto>(request);
            requestDto.ActionBy = SessionDetails.UserId;
            requestDto.ActionBy = SessionDetails.UserId;
            var result = await _userService.ChangePasswordAsync(requestDto);
            return APIResponse(result, "Password changed successfully.");
        }
        
        [HttpPost("DeactivateUserAsync")]
        public async Task<Dictionary<string, object>> DeactivateUserAsync([FromBody] DeactivateUserRequest request)
        {
            var requestDto = _mapper.Map<DeactivateUserDto>(request);
            requestDto.ActionBy = SessionDetails.UserId;
            var result = await _userService.DeactivateUserAsync(requestDto);
            return APIResponse(result, "User deactivated successfully.");
        }

        [HttpPost("ActivateUserAsync")]
        public async Task<Dictionary<string, object>> ActivateUserAsync([FromBody] ActivateUserRequest request)
        {
            var requestDto = _mapper.Map<ActivateUserDto>(request);
            requestDto.ActionBy = SessionDetails.UserId;
            var result = await _userService.ActivateUserAsync(requestDto);
            return APIResponse(result, "User activated successfully.");
        }

        [HttpPost("GetCountryAsync")]
        public async Task<Dictionary<string, object>> GetCountryAsync()
        {
            var result = await _userService.GetCountryAsync();
            return APIResponse(result, "Success");
        }

        [HttpPost("GetLoginDetails")]
        public async Task<Dictionary<string, object>> GetLoginDetails()
        {
            var result = await _userService.GetLoginDetailsDto(new GetLoginDetailsDto()
            {
                ActionBy = SessionDetails.UserId
            });
            return APIResponse(result, "Success");
        }

        [HttpPost("SignInAsync")]
        public async Task<Dictionary<string, object>> SignInAsync(SignInRequest request)
        { 
            var result = await _userService.SignInAsync(new SignInDto()
            {
                Salutation = request.Salutation,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                ISDCode = request.ISDCode,
                MobileNumber = request.MobileNumber,
                Image = request.Image,
                ImageExtension = request.ImageExtension,
                ImageName = request.ImageName
            });

            return APIResponse(result, "Success");
        }

        [HttpPost("UpdateProfileAsync")]
        public async Task<Dictionary<string, object>> UpdateProfileAsync(SignInRequest request)
        {
            var result = await _userService.SignInAsync(new SignInDto()
            {
                Salutation = request.Salutation,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                ISDCode = request.ISDCode,
                MobileNumber = request.MobileNumber,
                Image = request.Image,
                ImageExtension = request.ImageExtension,
                ImageName = request.ImageName
            });

            return APIResponse(result, "Success");
        }
    }
}

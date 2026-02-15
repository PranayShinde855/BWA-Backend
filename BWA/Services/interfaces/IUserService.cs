using BWA.DomainEntities;
using BWA.ServiceEntities;

namespace BWA.Services.interfaces
{
    public interface IUserService
    {
        Task<PaginationResult<UserDto>> GetUsersAsync(GetUsersDto request);
        Task<GetUsersDetailsDto> GetUserByIdAsync(GetUserByIdDto request);
        Task<bool> AddUserAsync(AddUserDto request);
        Task<bool> UpdateUserAsync(UpdateUserDto request);
        Task<bool> DeleteUserAsync(DeleteUserDto request);
        Task<List<GetISDCodesDto>> GetISDCodesAsync();
        Task<List<RoleDto>> GetRolesAsync();
        Task<bool> ChangePasswordAsync(ChangePasswordDto request);
        Task<bool> DeactivateUserAsync(DeactivateUserDto requestDto);
        Task<bool> ActivateUserAsync(ActivateUserDto requestDto);
        Task<List<Country>> GetCountryAsync();
        Task<LoginDetailsDto> GetLoginDetailsDto(GetLoginDetailsDto requestDto);
        Task<bool> SignInAsync(SignInDto requestDto);
        Task<bool> UpdateProfileAsync(SignInDto requestDto);
    }
}

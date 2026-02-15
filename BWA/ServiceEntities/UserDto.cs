
using BWA.APIInfrastructure.Requests;

namespace BWA.ServiceEntities
{
    public class UserDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class GetUsersDto : CommonPaginationProperties
    {
    }
    public class GetUsersDetailsDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ISDCode { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }
    }
    public class AddUserDto
    {
        public int RoleId { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ISDCode { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }
    }
    public class GetUserByIdDto
    {
        public int Id { get; set; }
    }
    public class UpdateUserDto : GetUserByIdDto
    {
        public int RoleId { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ISDCode { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }
        public bool RemoveImage { get; set; } = false;
    }

    public class DeleteUserDto
    {
        public int Id { get; set; }
    }

    public class GetISDCodesDto
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
    }
    
    public class ChangePasswordDto
    {
        public int ActionBy { get; set; } = 0;
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
    
    public class DeactivateUserDto
    {
        public int ActionBy { get; set; } = 0;
        public int UserId { get; set; } = 0;
    }
    public class ActivateUserDto
    {
        public int ActionBy { get; set; } = 0;
        public int UserId { get; set; } = 0;
    }

    public class GetLoginDetailsDto
    {
        public int ActionBy { get; set; } = 0;
    }

    public class LoginDetailsDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }
    }

    public class SignInDto
    {
        public int Id { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ISDCode { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;

    }
}

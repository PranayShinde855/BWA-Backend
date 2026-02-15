namespace BWA.APIInfrastructure.Requests
{
    public class GetUsersRequest : CommonPaginationProperties
    {
    }
    public class AddUserRequest
    {
        public int RoleId { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ISDCode { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageExtension { get; set; } = string.Empty;
    }
    public class GetUserByIdRequest
    {
        public int Id { get; set; }
    }
    public class UpdateUserRequest : GetUserByIdRequest
    {
        public int RoleId { get; set; }
        public string Salutation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ISDCode { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageExtension { get; set; } = string.Empty;
        public bool RemoveImage { get; set; } = false;
    }

    public class DeleteUserRequest
    {
        public int Id { get; set; }
    }

    public class SignInRequest
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

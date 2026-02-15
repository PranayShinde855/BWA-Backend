namespace BWA.APIInfrastructure.Requests
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
    
    public class DeactivateUserRequest
    {
        public int UserId { get; set; }
    }

    public class ActivateUserRequest : DeactivateUserRequest
    {
    }
}

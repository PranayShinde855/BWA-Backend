namespace BWA.ServiceEntities
{
    public class AuthDto
    {
    }

    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LogOutDto
    {
        public int ActionBy { get; set; }
    }

    public class LoginDetails
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
    public class JWTTokenGenerationDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class MaintainLoginConnectionHistoryDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}

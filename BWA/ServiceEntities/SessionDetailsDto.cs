namespace BWA.ServiceEntities
{
    public class SessionDetailsDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public string IPAddress { get; set; }
    }

    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
    }
}

using BWA.ServiceEntities;
using BWA.Utility;

namespace BWA.Services.interfaces
{
    public interface IJwtService
    {
        string GenerateSecurityToken(SessionDetailsDto sessionDetailsDto, out DateTime expiresOn);
        SessionDetailsDto ValidateJwtToken(string token);
        RefreshToken GenerateRefreshToken(string ipAddress);
    }
}

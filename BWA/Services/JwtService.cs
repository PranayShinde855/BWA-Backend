using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BWA.DomainEntities;
using BWA.ServiceEntities;
using BWA.Services.interfaces;
using BWA.Utility;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Constants = BWA.Utility.Constants;

namespace BWA.Services
{
    public class JwtService : IJwtService
    {
        private readonly AppSettings _appSettings;
        public JwtService(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
        }
        public string GenerateSecurityToken(SessionDetailsDto sessionDetailsDto, out DateTime expiresOn)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This_is_Key"));
            expiresOn = DateTime.UtcNow.AddMinutes(double.Parse(_appSettings.JwtExpiryInMinutes));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(Constants.JwtTokenClaimKey, System.Text.Json.JsonSerializer.Serialize(sessionDetailsDto))
                }),
                Expires = expiresOn,
                //SigningCredentials = creds
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            //tokenHandler.ValidateToken(token, new TokenValidationParameters
            //{
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = new SymmetricSecurityKey(key),
            //    ValidateIssuer = false,
            //    ValidateAudience = false,
            //    ClockSkew = TimeSpan.Zero
            //}, out SecurityToken validatedToken);

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public SessionDetailsDto ValidateJwtToken(string token)
        {
            if (token == null)
                throw new Exception("Invalid Token.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var sessionDetailsDto = JsonConvert.DeserializeObject<SessionDetailsDto>
                    (jwtToken.Claims.First(x => x.Type == Constants.JwtTokenClaimKey).Value);

                return sessionDetailsDto;
            }
            catch
            {
                return null;
            }
        }
        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            // generate token that is valid for 7 days
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
            return refreshToken;
        }
    }
}

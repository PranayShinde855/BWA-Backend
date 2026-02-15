using BWA.ServiceEntities;
using BWA.Utility;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace BWA.APIInfrastructure.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachUserToContextAsync(context, token);

            await _next(context);
        }

        private async Task AttachUserToContextAsync(HttpContext context, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            context.Items[Constants.JwtTokenClaimKey] = jwtToken.Claims.First(x => x.Type == Constants.JwtTokenClaimKey).Value;
            var userdetails = jwtToken.Claims.First(x => x.Type == Constants.JwtTokenClaimKey).Value;
            //var userRole = JsonSerializer.Deserialize<UserRole>(userdetails);
            var user = JsonSerializer.Deserialize<UserSessonDetailsDto>(userdetails);
        }

    }
}

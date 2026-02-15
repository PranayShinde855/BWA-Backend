using System.Security.Claims;
using BWA.APIInfrastructure.Attributes;
using BWA.Database.Infrastructure;
using BWA.ServiceEntities;
using BWA.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BWA.APIInfrastructure.Filters
{
    public class BWAAuthorizationHandler : AttributeAuthorizationHandler<BWAAuthorizationRequirement, BWAAuthorizationAttribute>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IUnitOfWork _unitOfWork;

        public BWAAuthorizationHandler(IHttpContextAccessor accessor,
            IUnitOfWork unitOfWork)
        {
            _accessor = accessor;
            _unitOfWork = unitOfWork;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            BWAAuthorizationRequirement requirement, IEnumerable<BWAAuthorizationAttribute> attributes)
        {
            foreach (var permissionAttribute in attributes)
            {
                if (!await AuthorizeAsync(context.User, permissionAttribute.Name))
                {
                    context.Fail();
                    throw new UnauthorizedAccessException();
                }
            }
            context.Succeed(requirement);
        }

        private async Task<bool> AuthorizeAsync(ClaimsPrincipal user, string permission = null)
        {
            var token = _accessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!user.Identity.IsAuthenticated)
                return false;

            var sessionDetails = (JsonConvert.DeserializeObject<SessionDetailsDto>(_accessor.HttpContext.Items[Constants.JwtTokenClaimKey].ToString()));

            if (!await _unitOfWork.ConnectionRepository.GetAllAsQueryable().AnyAsync(c => c.JWTToken == token))
            {
                throw new UnauthorizedAccessException("MMP6008");
            }

            if (_accessor.HttpContext.Items[Constants.JwtTokenClaimKey] == null)
                return false;

            var userDetails = await _unitOfWork.UserRepository.GetAllAsQueryable()
                .Include(x => x.Role)
                .Where(c => c.Id == sessionDetails.UserId)
                .FirstOrDefaultAsync();

            if (userDetails == null)
                return false;

            return true;
        }
    }

    public class BWAAuthorizationRequirement : IAuthorizationRequirement
    {
    }
}

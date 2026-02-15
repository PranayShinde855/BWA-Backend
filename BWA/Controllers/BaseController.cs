using AutoMapper;
using BWA.ServiceEntities;
using BWA.Services.interfaces;
using BWA.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BWA.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly ILogger _logger;
        protected readonly IMapper _mapper;
        protected readonly IHttpContextAccessor _accessor;
        protected readonly ICommonService _commonService;

        public BaseController(ILogger logger, IMapper mapper, IHttpContextAccessor accessor, ICommonService commonService)
        {
            _logger = logger;
            _mapper = mapper;
            _accessor = accessor;
            _commonService = commonService;
            Utils.IpAddress = _accessor.HttpContext.GetServerVariable("REMOTE_ADDR");
            Utils.LocalTime = _accessor.HttpContext!.Request.Headers["LocalTime"].ToString();
        }
        public SessionDetailsDto SessionDetails => JsonConvert.DeserializeObject<SessionDetailsDto>(ContextItem(Constants.JwtTokenClaimKey));
        protected Dictionary<string, object> APIResponse(object result, string msgCode = "")
        {
            var _result = new Dictionary<string, object>
            {
                { Constants.RESPONSE_DATA_FIELD, result }
            };
            if (!string.IsNullOrEmpty(msgCode))
            {
                _result.Add(Constants.RESPONSE_MESSAGE_FIELD, ContentLoader.ReturnLanguageData(msgCode));
            }
            else
            {
                _result.Add(Constants.RESPONSE_MESSAGE_FIELD, "");
            }
            return _result;
        }
        protected Dictionary<string, object> APIResponse(object result, string messageCode, string moduleName, string status, string otherStatus = "")
        {
            var _result = new Dictionary<string, object>
            {
                { Constants.RESPONSE_DATA_FIELD, result }
            };
            if (!string.IsNullOrEmpty(messageCode))
            {
                var message = ContentLoader.ReturnLanguageData(messageCode);
                _result.Add(Constants.RESPONSE_MESSAGE_FIELD, message);
            }
            return _result;
        }
        protected Dictionary<string, object> APIResponse(bool result, string failMsgCode, string successMsgCode)
        {
            return new Dictionary<string, object>
            {
                { Constants.RESPONSE_DATA_FIELD, result },
                { Constants.RESPONSE_MESSAGE_FIELD, ContentLoader.ReturnLanguageData(result ? successMsgCode : failMsgCode) }
            };
        }
        protected async Task LogInformation<T>(string accessAction) where T : class
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                { "UserId", SessionDetails.UserId },
                { "RoleId", SessionDetails.RoleId},
                { "IPAddress",_accessor.HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR") ?? SessionDetails.IPAddress   },
                { "PageAccess",  RequestHeader("X-Current-Page")}

            }
            ))
            {
                _logger.LogInformation(accessAction.ToString(), _logger);
            }

        }

        #region Private Methods
        private string RequestHeader(string headerName) => Convert.ToString(_accessor.HttpContext!.Request.Headers[headerName]);
        private string ContextItem(string itemName)
        {
            if (_accessor.HttpContext!.Items[itemName] != null)
                return Convert.ToString(_accessor.HttpContext!.Items[itemName]);
            throw new UnauthorizedAccessException("Unauthorized access");
        }
        #endregion
    }
}

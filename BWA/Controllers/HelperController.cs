using AutoMapper;
using BWA.APIInfrastructure.Attributes;
using BWA.Services.interfaces;
using BWA.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;

namespace BWA.Controllers
{
    [BWAAuthorization]
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : BaseController
    {
        public HelperController(ILogger<HelperController> logger, IMapper mapper, IHttpContextAccessor accessor, ICommonService commonService) : base(logger, mapper, accessor, commonService)
        {
        }

        [HttpPost("Encrypt")]
        public async Task<Dictionary<string, object>> Encrypt(string encryptString)
        {
            return APIResponse(Utils.Encrypt(encryptString), "Success");
        }

        [HttpPost("Decrypt")]
        public async Task<Dictionary<string, object>> Decrypt(string encryptString)
        {
            return APIResponse(Utils.Decrypt(encryptString), "Success");
        }
    }
}

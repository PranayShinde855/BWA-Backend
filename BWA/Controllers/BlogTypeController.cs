using AutoMapper;
using BWA.APIInfrastructure.Attributes;
using BWA.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BWA.Controllers
{
    [BWAAuthorization]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogTypeController : BaseController
    {
        private ICategoryService _categoryService;
        public BlogTypeController(ICategoryService categoryService, ILogger<BlogController> logger, IMapper mapper
            , IHttpContextAccessor accessor, ICommonService commonService)
             : base(logger, mapper, accessor, commonService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpPost("GetBlogTypesAsync")]
        public async  Task<Dictionary<string, object>> GetBlogTypesAsync()
        {
            var result = await _categoryService.GetBlogTypes();
            return APIResponse(result, "Success");
        }
    }
}

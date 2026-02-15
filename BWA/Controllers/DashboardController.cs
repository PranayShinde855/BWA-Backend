using AutoMapper;
using BWA.APIInfrastructure.Attributes;
using BWA.APIInfrastructure.Requests;
using BWA.ServiceEntities;
using BWA.Services;
using BWA.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BWA.Controllers
{
    [BWAAuthorization]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService, ILogger<BlogController> logger, IMapper mapper
           , IHttpContextAccessor accessor, ICommonService commonService)
            : base(logger, mapper, accessor, commonService)
        {
            _dashboardService = dashboardService;
        }

        [HttpPost("GetCountAsync")]
        public async Task<Dictionary<string, object>> GetCountAsync()
        {
            var posts = await _dashboardService.GetDashboardCountAsync();
            //return Ok(posts);
            return APIResponse(posts, "Success");
        }

        [HttpPost("GetBlogAsync")]
        public async Task<Dictionary<string, object>> GetBlogAsync(DashboardRequest request)
        {
            var posts = await _dashboardService.GetDashboardListAsync(new DashboardDto()
            {
                Id = request.Id
            });
            return APIResponse(posts, "Success");
        }

        [HttpPost("GetGraphBlogPostPerformanceAsync")]
        public async Task<Dictionary<string, object>> GetGraphDataAsync(GetDashboardPostPerformanceDataRequest request)
        {
            var posts = await _dashboardService.GetGraphBlogPostPerformanceAsync(new requestGraphDto()
            {
                Request = request.Year == null ? DateTime.Now : request.Year.Value,
            });            
            return APIResponse(posts, "Success");
        }

        [HttpPost("GetGraphCategoryPerformanceAsync")]
        public async Task<Dictionary<string, object>> GetGraphCategoryPerformanceAsync(GetDashboardPostPerformanceDataRequest request)
        {
            var posts = await _dashboardService.GetGraphCategoryPerformanceAsync(new requestGraphCategoryDto()
            {
                CategoryId = request.Id,
                Request = request.Year == null ? DateTime.Now : request.Year.Value,
            });
            return APIResponse(posts, "Success");
        }
    }
}

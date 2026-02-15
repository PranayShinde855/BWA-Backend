using AutoMapper;
using BWA.APIInfrastructure.Attributes;
using BWA.APIInfrastructure.Requests;
using BWA.ServiceEntities;
using BWA.Services.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BWA.Controllers
{
    [BWAAuthorization]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseController
    {        
        protected readonly IBlogService _blogService;

        public BlogController(IBlogService blogService, ILogger<BlogController> logger, IMapper mapper
            , IHttpContextAccessor accessor, ICommonService commonService)
            : base(logger, mapper, accessor, commonService)
        {
            _blogService = blogService;
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("GetAsync")]
        public async Task<Dictionary<string, object>> GetAsync(GetBlogsRequest request)
        {
            var posts = await _blogService.GetBlogsAsync(new GetBlogsDto()
            {
                GlobalSearch = request.GlobalSearch,
                OrderBy = request.OrderBy,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                ActionBy = SessionDetails.UserId,
                Role = SessionDetails.Role
            });
            //return Ok(posts);
            return APIResponse(posts, "Success");
        }

        [HttpPost("GetByIdAsync")]
        public async  Task<Dictionary<string, object>> GetByIdAsync(GetBlogByIdRequest request)
        {
            var post = await _blogService.GetBlogByIdAsync(new GetBlogByIdDto()
            {
                Id = request.Id
            });

            //if (post == null) return NotFound();
            //return Ok(post);
            return APIResponse(post, "Success");
        }

        //[Authorize("")]
        [HttpPost("AddAsync")]
        public async  Task<Dictionary<string, object>> AddAsync([FromBody] AddBlogRequest request)
        {
            var result = await _blogService.AddBlogAsync(new AddBlogDto()
            {
                Content = request.Content,
                Title = request.Title,
                ActionBy = SessionDetails.UserId,
                CategoryId = request.CategoryId,
                Image = request.Image,
                ImageName = request.ImageName,
                ImageExtension = request.ImageExtension
            });

            //return Ok(result);
            return APIResponse(result, "Success");
        }
        
        [HttpPost("UpdateAsync")]
        public async  Task<Dictionary<string, object>> UpdateAsync([FromBody] UpdateBlogRequest request)
        {
            var result = await _blogService.UpdateBlogAsync(new UpdateBlogDto()
            {
                Id = request.Id,
                Content = request.Content,
                Title = request.Title,
                ActionBy = SessionDetails.UserId,
                CategoryId = request.CategoryId,
                Image = request.Image,
                ImageName = request.ImageName,
                ImageExtension = request.ImageExtension,
                Role = SessionDetails.Role
            });

            //return Ok(result);
            return APIResponse(result, "Success");
        }

        [HttpPost("DeleteAsync")]
        public async  Task<Dictionary<string, object>> DeleteAsync([FromBody] DeleteBlogRequest request)
        {
            var post = await _blogService.DeleteBlogAsync(new DeleteBlogDto()
            {
                Id = request.Id
            });

            //if (!post) return NotFound();
            
            //return Ok(post);
            return APIResponse(post, "Success");

        }

         [HttpPost("GetUserBlogByIdAsync")]
        public async  Task<Dictionary<string, object>> GetUserBlogByIdAsync(GetBlogByIdRequest request)
        {
            var post = await _blogService.GetUserBlogByIdAsync(new GetBlogByIdDto()
            {
                Id = request.Id
            });

            return APIResponse(post, "Success");
        }

        [HttpPost("GetUserBlogByCategoryIdAsync")]
        public async Task<Dictionary<string, object>> GetUserBlogByCategoryIdAsync(GetBlogByCategoryIdRequest request)
        {
            var post = await _blogService.GetUserBlogByCategoryIdAsync(new GetBlogByCategoryIdDto()
            {
                CategoryId = request.CategoryId
            });

            return APIResponse(post, "Success");
        }
    }
}

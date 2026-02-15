using System.Reflection.Metadata;
using BWA.Database.Infrastructure;
using BWA.DomainEntities;
using BWA.ServiceEntities;
using BWA.Services.interfaces;
using BWA.Utility;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BWA.Services
{
    public class BlogService : IBlogService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ICommonService _commonService;

        public BlogService(IUnitOfWork unitOfWork, ICommonService commonService)
        {
            _unitOfWork = unitOfWork;
            _commonService = commonService;
        }

        public async Task<bool> AddBlogAsync(AddBlogDto requestDto)
        {
            var addBlod = new BlogPost
            {
                CategoryId = requestDto.CategoryId,
                Title = requestDto.Title,
                Content = requestDto.Content,
                Image = new byte[0],
                CreatedBy = requestDto.ActionBy,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            if (!string.IsNullOrEmpty(requestDto.Image) && requestDto.Image.ToLower() != "String".ToLower())
            {
                addBlod.ImageName = requestDto.ImageName;
                addBlod.ImageExtension = requestDto.ImageExtension;
                addBlod.Image = Utils.Base64ToBytes(requestDto.Image);
            }

            await _unitOfWork.BlogRepositry.AddAsync(addBlod);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteBlogAsync(DeleteBlogDto requestDto)
        {
            var blog = await _unitOfWork.BlogRepositry.GetAllAsQueryable()
                .Where(x => x.Id == requestDto.Id
                && x.IsActive)
                .FirstOrDefaultAsync();

            if (blog == null)
                throw new KeyNotFoundException("Not Found");

            blog.IsActive = false;
            blog.ModifiedBy = requestDto.ActionBy;
            blog.ModifiedDate = Utils.CurrentDateTime;
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<GetBlogsDetailsDto> GetBlogByIdAsync(GetBlogByIdDto requestDto)
        {
            var blog = await _unitOfWork.BlogRepositry.GetAllAsQueryable()
              .Where(x => x.Id == requestDto.Id
              && x.IsActive)
              .FirstOrDefaultAsync();

            if (blog == null)
                throw new KeyNotFoundException("Not Found");

            return new GetBlogsDetailsDto()
            {
                CategoryId = blog.CategoryId == null ? 0 : blog.CategoryId,
                Title = blog.Title,
                Content = blog.Content,
                Image = (blog.Image == null || blog.Image.Length <= 0) ? "" : Utils.BytesToBase64(blog.Image),
                ImageName = blog.ImageName != null && blog.ImageName.Length > 0 ? blog.ImageName : "",
                ImageExtension = blog.ImageExtension != null && blog.ImageExtension.Length > 0 ? blog.ImageExtension : ""
            };
        }

        public async Task<PaginationResult<BlogDto>> GetBlogsAsync(GetBlogsDto requestDto)
        {
            var result = _unitOfWork.BlogRepositry.GetAllAsQueryable()
                .Include(x => x.CreaedByUser)
                .Where(x => requestDto.Role.ToLower() == nameof(UserRole.User).ToLower() ?
                (x.CreatedBy == requestDto.ActionBy) : (x.CreatedBy > 0)
                && x.IsActive == true)
                .Select(x => new BlogDto()
                {
                    Id = x.Id,
                    CreatedBy = Utils.GetFullName(x.CreaedByUser.Salutation, x.CreaedByUser.FirstName, x.CreaedByUser.LastName),
                    CreatedDate = x.CreatedDate,
                    Category = x.Category == null ? "-" : x.Category.Name,
                    Title = x.Title
                });

            if (!string.IsNullOrEmpty(requestDto.GlobalSearch))
                result = result.Where(x => x.Title.Contains(requestDto.GlobalSearch));

            if (!string.IsNullOrEmpty(requestDto.OrderBy))
                result = result.OrderBy(x => x.Title);

            return await _commonService.Pagination(result, requestDto.PageIndex, requestDto.PageSize);
        }

        public async Task<bool> UpdateBlogAsync(UpdateBlogDto requestDto)
        {
            var blog = await _unitOfWork.BlogRepositry.GetAllAsQueryable()
               .Where(x => x.Id == requestDto.Id
               && x.IsActive
               && requestDto.Role.ToLower() == nameof(UserRole.User).ToLower() ?
                x.CreatedBy == requestDto.ActionBy : x.CreatedBy > 0)
               .FirstOrDefaultAsync();

            if (blog == null)
                throw new KeyNotFoundException("Not Found");

            blog.CategoryId = requestDto.CategoryId;
            blog.Title = requestDto.Title;
            blog.Content = requestDto.Content;
            blog.ModifiedBy = requestDto.ActionBy;
            blog.ModifiedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(requestDto.ImageName))
            {
                blog.ImageName = null;
                blog.ImageExtension = null;
                blog.Image = new byte[0];
            }
            else
            {
                blog.ImageName = requestDto.ImageName;
                blog.ImageExtension = requestDto.ImageExtension;
                blog.Image = Utils.Base64ToBytes(requestDto.Image);
            }

            _unitOfWork.BlogRepositry.Update(blog);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<GetUserBlogsDetailsDto> GetUserBlogByIdAsync(GetBlogByIdDto requestDto)
        {
            var blog = await _unitOfWork.BlogRepositry.GetAllAsQueryable()
              .Include(x => x.Category)
              .Where(x => x.Id == requestDto.Id
              && x.IsActive)
              .FirstOrDefaultAsync();

            if (blog == null)
                throw new KeyNotFoundException("Not Found");

            return new GetUserBlogsDetailsDto()
            {
                CategoryId = blog.CategoryId == null ? 0 : blog.CategoryId,
                Category = blog.CategoryId == null ? "-" : blog.Category.Name,
                Title = blog.Title,
                Content = blog.Content,
                Image = (blog.Image == null || blog.Image.Length <= 0) ? "" : Utils.BytesToBase64(blog.Image),
                ImageName = blog.ImageName != null && blog.ImageName.Length > 0 ? blog.ImageName : "",
                ImageExtension = blog.ImageExtension != null && blog.ImageExtension.Length > 0 ? blog.ImageExtension : "",
                CreatedDate = blog.CreatedDate.Date
            };
        }
        public async Task<GetUserBlogsDetailsDto> GetUserBlogByCategoryIdAsync(GetBlogByCategoryIdDto requestDto)
        {
            var blog = await _unitOfWork.BlogRepositry.GetAllAsQueryable()
              .Include(x => x.Category)
              .Where(x => requestDto.CategoryId > 0 ? x.CategoryId == requestDto.CategoryId : x.CategoryId > 0
              && x.IsActive)
              .FirstOrDefaultAsync();

            if (blog == null)
                throw new KeyNotFoundException("Not Found");

            return new GetUserBlogsDetailsDto()
            {
                CategoryId = blog.CategoryId == null ? 0 : blog.CategoryId,
                Category = blog.CategoryId == null ? "-" : blog.Category.Name,
                Title = blog.Title,
                Content = blog.Content,
                Image = (blog.Image == null || blog.Image.Length <= 0) ? "" : Utils.BytesToBase64(blog.Image),
                ImageName = blog.ImageName != null && blog.ImageName.Length > 0 ? blog.ImageName : "",
                ImageExtension = blog.ImageExtension != null && blog.ImageExtension.Length > 0 ? blog.ImageExtension : "",
                CreatedDate = blog.CreatedDate.Date
            };
        }
    }
}

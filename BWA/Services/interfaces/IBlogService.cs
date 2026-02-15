using BWA.ServiceEntities;

namespace BWA.Services.interfaces
{
    public interface IBlogService
    {
        Task<PaginationResult<BlogDto>> GetBlogsAsync(GetBlogsDto request);
        Task<GetBlogsDetailsDto> GetBlogByIdAsync(GetBlogByIdDto request);
        Task<bool> AddBlogAsync(AddBlogDto request);
        Task<bool> UpdateBlogAsync(UpdateBlogDto request);
        Task<bool> DeleteBlogAsync(DeleteBlogDto request);
        Task<GetUserBlogsDetailsDto> GetUserBlogByIdAsync(GetBlogByIdDto request);
        Task<GetUserBlogsDetailsDto> GetUserBlogByCategoryIdAsync(GetBlogByCategoryIdDto requestDto);
    }
}

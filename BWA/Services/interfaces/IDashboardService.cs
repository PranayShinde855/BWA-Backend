using BWA.ServiceEntities;

namespace BWA.Services.interfaces
{
    public interface IDashboardService
    {
        Task<DashboardCountsDto> GetDashboardCountAsync();
        Task<List<DashboardBlogsDto>> GetDashboardListAsync(DashboardDto requestDto);
        Task<DashboardPostPerformanceDto> GetGraphBlogPostPerformanceAsync(requestGraphDto requestDto);
        Task<DashboardPostPerformanceDto> GetGraphCategoryPerformanceAsync(requestGraphCategoryDto requestDto);
    }
}

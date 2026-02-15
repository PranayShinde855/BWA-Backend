using BWA.Database.Infrastructure;
using BWA.ServiceEntities;
using BWA.Services.interfaces;
using BWA.Utility;
using Microsoft.EntityFrameworkCore;

namespace BWA.Services
{
    public class DashboardService : IDashboardService
    {
        protected readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardCountsDto> GetDashboardCountAsync()
        {
            return new DashboardCountsDto()
            {
                BlogsCount = await _unitOfWork.BlogRepositry.GetAllAsQueryable().Where(x => x.IsActive).CountAsync(),
                TypesCount = await _unitOfWork.CategoryRepository.GetAllAsQueryable().CountAsync(),
                UsersCount = await _unitOfWork.UserRepository.GetAllAsQueryable().Where(x => x.IsActive).CountAsync(),
            };
        }

        public async Task<List<DashboardBlogsDto>> GetDashboardListAsync(DashboardDto requestDto)
        {
            return await _unitOfWork.BlogRepositry.GetAllAsQueryable()
                .Where(x => x.IsActive
                && requestDto.Id > 0 ? x.CategoryId == requestDto.Id : x.CategoryId > 0)
                .Select(x => new DashboardBlogsDto()
                {
                    id = x.Id,
                    Title = x.Title,
                    Description = x.Content,
                    Image = x.Image != null && x.Image.Length > 0 ? Utils.BytesToBase64(x.Image) : "",
                    ImageExtension = x.ImageExtension != null ? x.ImageExtension : "",
                    ImageName = x.ImageName != null ? x.ImageName : "",
                })
                .ToListAsync();
        }

        public async Task<DashboardPostPerformanceDto> GetGraphBlogPostPerformanceAsync(requestGraphDto requestDto)
        {
            int index = 0;

            var dataMonthly = await _unitOfWork.BlogRepositry.GetAllAsQueryable()
              .Where(x => x.IsActive
              && x.CreatedDate.Year == requestDto.Request.Year)
              .Select(x => new
              {
                  Id = x.Id,
                  CreatedDate = x.CreatedDate.Date
              }).ToListAsync();

            var yesrWiseData = new List<GraphData>();

            for (int i = 0; i < 12; i++)
            {
                var month = new DateTime(requestDto.Request.Year, i + 1, 1);
                yesrWiseData.Add(new GraphData()
                {
                    Data = Enum.GetName(typeof(Year), i),
                    Value = Convert.ToString(dataMonthly.Where(x=>x.CreatedDate.Month == month.Month).Count())
                    //Value = Convert.ToString(i + 1)
                });
            }

            var data = new DashboardPostPerformanceDto()
            {
                Id = 0,
                DataFor = "Chart",
                Data = yesrWiseData
            };

            return data;
        }

        public async Task<DashboardPostPerformanceDto> GetGraphCategoryPerformanceAsync(requestGraphCategoryDto requestDto)
        {
            int index = 0;

            var dataMonthly = await _unitOfWork.BlogRepositry.GetAllAsQueryable()
              .Where(x => x.IsActive
              && x.CreatedDate.Year == requestDto.Request.Year
              && x.CategoryId == requestDto.CategoryId)
              .Select(x => new
              {
                  Id = x.Id,
                  CreatedDate = x.CreatedDate.Date
              }).ToListAsync();

            var yesrWiseData = new List<GraphData>();

            for (int i = 0; i < 12; i++)
            {
                var month = new DateTime(requestDto.Request.Year, i + 1, 1);
                yesrWiseData.Add(new GraphData()
                {
                    Data = Enum.GetName(typeof(Year), i),
                    Value = Convert.ToString(dataMonthly.Where(x => x.CreatedDate.Month == month.Month).Count())
                });
            }

            var data = new DashboardPostPerformanceDto()
            {
                Id = 0,
                DataFor = "Chart",
                Data = yesrWiseData
            };

            return data;
        }
    }
}

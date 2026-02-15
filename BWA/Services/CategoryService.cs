using BWA.Database.Infrastructure;
using BWA.ServiceEntities;
using BWA.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace BWA.Services
{
    public class CategoryService : ICategoryService
    {
        protected IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryDto>> GetBlogTypes()
        {
            return await _unitOfWork.CategoryRepository.GetAllAsQueryable()
                .Select(x => new CategoryDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();
        }
    }
}

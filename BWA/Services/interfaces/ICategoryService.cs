using BWA.ServiceEntities;

namespace BWA.Services.interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetBlogTypes();
    }
}

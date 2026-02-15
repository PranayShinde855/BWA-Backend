using BWA.ServiceEntities;

namespace BWA.Services.interfaces
{
    public interface ICommonService
    {
        Task<PaginationResult<T>> Pagination<T>(IQueryable<T> query, int pageIndex, int pageSize) where T : class;
        IQueryable<T> Sorting<T>(IQueryable<T> query, string orderBy, string orderDirection);
        IQueryable<T> SortingByTwoKey<T>(IQueryable<T> query, string firstOrderByValue, string secondOrderByValue, string orderDirection);
    }
}

using System.Linq.Expressions;

namespace BWA.Database.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        Task AddAsync(T entity);

        Task<T> AddEntityAsync(T entity);

        IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int skip = 0, int take = 10);

        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<T> GetByIdAsync(object id);
        T GetById(Guid id);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
        IQueryable<T> GetAllAsQueryable(string navigationPropertyInclude1, string navigationPropertyInclude2);
        IQueryable<T> Query(bool eager = false);
        IQueryable<T> GetAllAsQueryable();
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        void UpdateRange(IEnumerable<T> entities);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool eager = false);
        //Task<T> GetByIdAsync(int id);
        //Task<IEnumerable<T>> GetAllAsync();

        //Task<IEnumerable<T>> GetAllAsync(
        //Expression<Func<T, bool>>? filter = null,
        //Expression<Func<T, bool>>? search = null,
        //Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        //string includeProperties = "",
        //int? skip = null,
        //int? take = null);

        //Task<T> AddAsync(T entity);
        //Task AddRangeAsync(IEnumerable<T> entities);
        //T Update(T entity);
        //void UpdateRange(IEnumerable<T> entities);
        //IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        //void Delete(T entity);
        //void DeleteRange(IEnumerable<T> entities);
    }

}

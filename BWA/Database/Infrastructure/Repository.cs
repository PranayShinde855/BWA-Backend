using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using BWA.Database.Contexts;
using System;

namespace BWA.Database.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly BWAContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(BWAContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }



        public void Add(T entity)
        {
            var dbSet = _context.Set<T>();
            dbSet.Add(entity);
        }

        public virtual async Task AddAsync(T entity)
        {
            var dbSet = _context.Set<T>();
            await dbSet.AddAsync(entity);


        }
        public virtual async Task<T> AddEntityAsync(T entity)
        {
            var dbSet = _context.Set<T>();
            await dbSet.AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int skip = 0, int take = 10)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            return orderBy != null ? orderBy(query).Skip(skip).Take(take) : query;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            return orderBy != null ? orderBy(query) : query;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual IQueryable<T> GetAllAsQueryable(string navigationPropertyInclude1, string navigationPropertyInclude2)
        {
            return _context.Set<T>().Include(navigationPropertyInclude1).Include(navigationPropertyInclude2).AsQueryable();
        }

        public virtual IQueryable<T> Query(bool eager = false)
        {
            var query = _context.Set<T>().AsQueryable();
            if (eager)
            {
                foreach (var property in _context.Model.FindEntityType(typeof(T)).GetNavigations())
                    query = query.Include(property.Name);
            }
            return query;
        }

        public virtual IQueryable<T> GetAllAsQueryable()
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            var dbSet = _context.Set<T>();
            await dbSet.AddRangeAsync(entities);
            return entities;
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            var dbSet = _context.Set<T>();
            dbSet.UpdateRange(entities);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.CountAsync();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            var dbSet = _context.Set<T>();
            return await dbSet.AnyAsync(predicate);
        }

        public virtual async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool eager = false)
        {
            return await Query(eager).FirstOrDefaultAsync(predicate);
        }

        //https://github.com/thepirat000/Audit.NET/issues/53


        //public async Task<IEnumerable<T>> GetAllAsync(
        //Expression<Func<T, bool>>? filter = null,
        //Expression<Func<T, bool>>? search = null,
        //Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        //string includeProperties = "",
        //int? skip = null,
        //int? take = null)
        //{
        //    IQueryable<T> query = _dbSet;

        //    if (filter != null)
        //        query = query.Where(filter);

        //    if (search != null)
        //        query = query.Where(search);

        //    foreach (var includeProperty in includeProperties.Split(
        //        new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        query = query.Include(includeProperty);
        //    }

        //    if (orderBy != null)
        //        query = orderBy(query);

        //    if (skip.HasValue)
        //        query = query.Skip(skip.Value);

        //    if (take.HasValue)
        //        query = query.Take(take.Value);

        //    return await query.ToListAsync();
        //}

        //public async Task<T> AddAsync(T entity)
        //{
        //    await _context.Set<T>().AddAsync(entity);
        //    return entity;
        //}
        //public async Task AddRangeAsync(IEnumerable<T> entities)
        //{
        //    await _context.Set<T>().AddRangeAsync(entities);
        //}
        //public T UpdateAsync(T entity)
        //{
        //    _context.Set<T>().Update(entity);
        //    return entity;
        //}
        //public void UpdateRange(IEnumerable<T> entities)
        //{
        //    _context.Set<T>().AddRange(entities);
        //}
        //public async Task<IEnumerable<T>> GetAllAsync()
        //{
        //    return await _context.Set<T>().ToListAsync();
        //}
        //public async Task<T> GetByIdAsync(int id)
        //{
        //    return await _context.Set<T>().FindAsync(id);
        //}
        //public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        //{
        //    return _context.Set<T>().Where(expression);
        //}
        //public void Delete(T entity)
        //{
        //    _context.Set<T>().Remove(entity);
        //}
        //public void DeleteRange(IEnumerable<T> entities)
        //{
        //    _context.Set<T>().RemoveRange(entities);
        //}

        //public T Update(T entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

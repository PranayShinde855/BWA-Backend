using BWA.ServiceEntities;
using System.Linq.Expressions;
using System.Reflection;
using BWA.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace BWA.Services
{
    public class CommonService : ICommonService
    {
        public async Task<PaginationResult<T>> Pagination<T>(IQueryable<T> query, int pageIndex, int pageSize) where T : class
        {
            if (pageIndex <= 0 || pageSize <= 0)
            {
                pageIndex = 1;
                pageSize = 10;
                //throw new BadHttpRequestException("PageIndex and PageSize must be greater than zero.");
            }

            var totalCount = await query.CountAsync();
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);

            var dataQuery = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var dataList = await dataQuery.ToListAsync();

            return new PaginationResult<T>
            {
                List = dataList,
                TotalCount = totalCount,
                PageCount = pageCount
            };
        }
        public IQueryable<T> Sorting<T>(IQueryable<T> query, string orderBy, string orderDirection)
        {
            var propInfo = typeof(T).GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propInfo == null)
            {
                propInfo = typeof(T).GetProperty("Id");
            }
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.Property(param, propInfo);
            if (propInfo.PropertyType == typeof(DateTime))
            {
                var sort = Expression.Lambda<Func<T, DateTime>>(prop, param);
                if (orderDirection.ToLower() == "   ")
                {
                    query = query.OrderBy(sort);
                }
                else
                {
                    query = query.OrderByDescending(sort);
                }
            }
            else
            {
                var sort = Expression.Lambda<Func<T, object>>(Expression.Convert(prop, typeof(object)), param);
                if (orderDirection.ToLower() == "asc")
                {
                    query = query.OrderBy(sort);
                }
                else
                {
                    query = query.OrderByDescending(sort);
                }
            }
            return query;
        }
        public IQueryable<T> SortingByTwoKey<T>(IQueryable<T> query, string firstOrderByValue, string secondOrderByValue, string orderDirection)
        {
            var firstvalueProperty = typeof(T).GetProperty(firstOrderByValue, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            var secondvalueProperty = typeof(T).GetProperty(firstOrderByValue, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (firstvalueProperty == null && secondvalueProperty == null)
            {
                firstvalueProperty = typeof(T).GetProperty("Id");
                secondvalueProperty = typeof(T).GetProperty("Id");
            }

            var param = Expression.Parameter(typeof(T), "x");
            var prop1 = Expression.Property(param, firstvalueProperty);
            var prop2 = Expression.Property(param, secondvalueProperty);

            var sort1 = Expression.Lambda<Func<T, object>>(Expression.Convert(prop1, typeof(object)), param);
            var sort2 = Expression.Lambda<Func<T, object>>(Expression.Convert(prop2, typeof(object)), param);

            if (orderDirection.ToLower() == "asc")
            {
                query = query.OrderByDescending(sort1)
                    .ThenBy(sort2);
            }
            else
            {
                query = query.OrderByDescending(sort1)
                    .ThenBy(sort2);
            }
            return query;
        }
    }
}

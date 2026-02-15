using BWA.Database.Contexts;
using BWA.Database.Infrastructure;
using BWA.Database.Interfaces;
using BWA.DomainEntities;

namespace BWA.Database.Repositories
{
    public class BlogRepository : Repository<BlogPost>, IBlogRepository
    {
        public BlogRepository(BWAContext context) : base(context)
        {
        }
    }
}

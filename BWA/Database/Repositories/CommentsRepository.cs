using BWA.Database.Contexts;
using BWA.Database.Infrastructure;
using BWA.Database.Interfaces;
using BWA.DomainEntities;

namespace BWA.Database.Repositories
{
    public class CommentsRepository : Repository<Comments>, ICommentsRepository
    {
        public CommentsRepository(BWAContext context) : base(context)
        {
        }
    }
}

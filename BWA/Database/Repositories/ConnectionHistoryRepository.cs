using BWA.Database.Contexts;
using BWA.Database.Infrastructure;
using BWA.Database.Interfaces;
using BWA.DomainEntities;

namespace BWA.Database.Repositories
{
    public class ConnectionHistoryRepository : Repository<ConnectionHistory>, IConnectionHistoryRepository
    {
        public ConnectionHistoryRepository(BWAContext context) : base(context)
        {
        }
    }
}

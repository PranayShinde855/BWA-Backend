using BWA.Database.Contexts;
using BWA.Database.Infrastructure;
using BWA.Database.Interfaces;
using BWA.DomainEntities;

namespace BWA.Database.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(BWAContext context) : base(context)
        {
        }
    }
}

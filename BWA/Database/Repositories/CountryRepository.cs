using BWA.Database.Contexts;
using BWA.Database.Infrastructure;
using BWA.Database.Interfaces;
using BWA.DomainEntities;

namespace BWA.Database.Repositories
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(BWAContext context) : base(context)
        {
        }
    }
}

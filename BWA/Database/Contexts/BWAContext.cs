using BWA.DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace BWA.Database.Contexts
{
    public class BWAContext : DbContext
    {
        public BWAContext(DbContextOptions<BWAContext> options) : base(options)
        {            
        }

        public DbSet<User> User { get; set; }
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Connection> Connection { get; set; }
        public DbSet<ConnectionHistory> ConnectionHistory { get; set; }
    }
}

using BWA.Database.Interfaces;

namespace BWA.Database.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        public IRoleRepository RoleRepository { get; set; }
        public IBlogRepository BlogRepositry { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public ICommentsRepository CommentsRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public ICountryRepository CountryRepository { get; set; }
        public IConnectionRepository ConnectionRepository { get; set; }
        public IConnectionHistoryRepository ConnectionHistoryRepository { get; set; }
        Task BeginTransactionAsync();
        Task RollbackTransactionAsync();
        Task CommitTransactionsAsync();
        Task<bool> SaveChangesAsync();
    }
}

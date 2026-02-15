using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using BWA.Database.Contexts;
using BWA.Database.Interfaces;

namespace BWA.Database.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BWAContext _context;
        private IDbContextTransaction? _transaction;
        public IRoleRepository RoleRepository { get; set; }
        public IBlogRepository BlogRepositry { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public ICommentsRepository CommentsRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public ICountryRepository CountryRepository { get; set; }
        public IConnectionRepository ConnectionRepository { get; set; }
        public IConnectionHistoryRepository ConnectionHistoryRepository { get; set; }
        public UnitOfWork(BWAContext context
        , IRoleRepository roleRepository
        , IBlogRepository blogRepositry
        , ICategoryRepository categoryRepository
        , ICommentsRepository commentsRepository
        , IUserRepository userRepository
        , ICountryRepository countryRepository
        , IConnectionRepository connectionRepository
        , IConnectionHistoryRepository connectionHistoryRepository)

        {
            _context = context;
            RoleRepository = roleRepository;
            BlogRepositry = blogRepositry;
            CategoryRepository = categoryRepository;
            CommentsRepository = commentsRepository;
            UserRepository = userRepository;
            CountryRepository = countryRepository;
            ConnectionRepository = connectionRepository;
            ConnectionHistoryRepository = connectionHistoryRepository;
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
                _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionsAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
            catch
            {
                if (_transaction != null)
                    await _transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}

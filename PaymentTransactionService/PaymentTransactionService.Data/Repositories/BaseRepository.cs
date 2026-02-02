using Microsoft.EntityFrameworkCore;
using PaymentTransactionService.Data.IRepositories;
using PaymentTransactionService.Data.Models;

namespace PaymentTransactionService.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly TransactionDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(TransactionDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();

        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            await Task.Delay(0);
            return _dbSet;
        }

        public async Task<T?> GetAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task InsertAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}

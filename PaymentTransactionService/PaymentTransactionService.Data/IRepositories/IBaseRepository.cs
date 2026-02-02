namespace PaymentTransactionService.Data.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAllAsync();
        Task<T?> GetAsync(long id);
        Task InsertAsync(T entity);
        Task<int> SaveChangesAsync();
    }
}

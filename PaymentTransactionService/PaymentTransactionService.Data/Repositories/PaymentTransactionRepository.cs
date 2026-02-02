using Microsoft.EntityFrameworkCore;
using PaymentTransactionService.Data.Enums;
using PaymentTransactionService.Data.IRepositories;
using PaymentTransactionService.Data.Models;
using System.Transactions;

namespace PaymentTransactionService.Data.Repositories
{
    public class PaymentTransactionRepository : BaseRepository<PaymentTransaction>, IPaymentTransactionRepository
    {
        private readonly TransactionDbContext _dbContext;
        public PaymentTransactionRepository(TransactionDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaymentTransaction?> GetByProviderReference(string providerReference)
        {
            return await _dbContext.PaymentTransactions.FirstOrDefaultAsync(x => x.ProviderReference == providerReference);
        }
    }
}

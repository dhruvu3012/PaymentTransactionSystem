using PaymentTransactionService.Data.Models;

namespace PaymentTransactionService.Data.IRepositories
{
    public interface IPaymentTransactionRepository : IBaseRepository<PaymentTransaction>
    {
        Task<PaymentTransaction?> GetByProviderReference(string providerReference);
    }
}

using PaymentTransactionService.Business.Models;
using PaymentTransactionService.Data.Models;

namespace PaymentTransactionService.Business.IServices
{
    public interface ITransactionService
    {
        Task<CreatePaymentResponseDto> CreatePayment(CreatePaymentDto input);
        Task<List<TransactionServiceDto>> GetAll();
        Task<bool> Payment(WebhookDto input);
        Task<bool> VerifyPayment(string providerReference);
    }
}

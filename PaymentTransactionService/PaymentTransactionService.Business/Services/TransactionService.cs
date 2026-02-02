using PaymentTransactionService.Business.IServices;
using PaymentTransactionService.Business.Models;
using PaymentTransactionService.Data.Enums;
using PaymentTransactionService.Data.IRepositories;
using PaymentTransactionService.Data.Models;

namespace PaymentTransactionService.Business.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IPaymentTransactionRepository _paymentRepository;
        
        public TransactionService(IPaymentTransactionRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<CreatePaymentResponseDto> CreatePayment(CreatePaymentDto input)
        {
            string orderId = "ORD_" + DateTime.UtcNow.Ticks.ToString();
            string providerReference = "PAY_" + DateTime.UtcNow.Ticks.ToString();
            await _paymentRepository.InsertAsync(new PaymentTransaction()
            {
                Amount = input.Amount,
                OrderId = orderId,
                ProviderReference = providerReference,
                Status = (int)PaymentStatus.Pending
            });
            await _paymentRepository.SaveChangesAsync();
            return new CreatePaymentResponseDto()
            {
                OrderId = orderId,
                Status = PaymentStatus.Pending.ToString(),
                Amount = input.Amount,
                ProviderReference = providerReference
            };
        }

        public async Task<List<TransactionServiceDto>> GetAll()
        {
            var result = await _paymentRepository.GetAllAsync();
            return result.Select(x => new TransactionServiceDto()
            {
                Amount = x.Amount,
                Id = x.Id,
                OrderId = x.OrderId,
                ProviderReference = x.ProviderReference,
                Status = (PaymentStatus)x.Status,
                UpdatedOn = x.UpdatedOn
            }).ToList();
        }

        public async Task<bool> Payment(WebhookDto input)
        {
            var getTrascation = await _paymentRepository.GetByProviderReference(input.ProviderReference);
            if(getTrascation!=null)
            {
                getTrascation.Status = (int)input.Status;
                getTrascation.UpdatedOn = DateTime.UtcNow;

                if (await _paymentRepository.SaveChangesAsync() > 0)
                    return true;
            }
            return false;
        }

        public async Task<bool> VerifyPayment(string providerReference)
        {
            var getTransction = await _paymentRepository.GetByProviderReference(providerReference);
            if (getTransction != null)
                return true;
            return false;
        }
    }
}

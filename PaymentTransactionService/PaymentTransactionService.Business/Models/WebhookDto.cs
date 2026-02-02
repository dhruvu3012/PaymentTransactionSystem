using PaymentTransactionService.Data.Enums;

namespace PaymentTransactionService.Business.Models
{
    public class WebhookDto
    {
        public string ProviderReference { get; set; }
        public PaymentStatus Status { get; set; }
    }
}

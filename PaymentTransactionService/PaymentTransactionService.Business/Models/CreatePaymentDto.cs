using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentTransactionService.Business.Models
{
    public class CreatePaymentDto
    {
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
    }

    public class CreatePaymentResponseDto
    {
        public string OrderId { get; set;}
        public string ProviderReference { get; set;}
        public decimal Amount { get; set; }
        public string Status { get; set;}
    }
}

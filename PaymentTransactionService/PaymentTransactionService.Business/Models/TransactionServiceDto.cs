using PaymentTransactionService.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentTransactionService.Business.Models
{
    public class TransactionServiceDto
    {
        public long Id { get; set; }
        public string OrderId { get; set; } = null!;
        public string? ProviderReference { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}

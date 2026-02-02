using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PaymentTransactionService.Data.Models;

[Table("PaymentTransaction")]
public partial class PaymentTransaction
{
    [Key]
    public long Id { get; set; }

    [StringLength(100)]
    public string OrderId { get; set; } = null!;

    [StringLength(100)]
    public string? ProviderReference { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public int Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedOn { get; set; }
}

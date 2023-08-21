using System;

namespace AcquiringBank.Domain.Payment.Transaction;

public record TransactionDetailsDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Merchant { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
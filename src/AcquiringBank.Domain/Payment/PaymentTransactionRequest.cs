using AcquiringBank.Domain.Payment.Transaction;

namespace AcquiringBank.Domain.Payment;

public record PaymentTransactionRequest
{
    public CardDetailsDto CardDetails { get; set; }
    public TransactionDetailsDto Details { get; set; }
}
using System.Collections.Generic;
using System.Linq;

namespace AcquiringBank.Domain.Payment;

public record PaymentTransactionResponse
{
    public bool IsApproved => !Validations.Any();
    public ICollection<TransactionValidationResult> Validations { get; set; } = new List<TransactionValidationResult>();
}

public struct TransactionValidationResult
{
    public static TransactionValidationResult FromSuccess()
    {
        return new TransactionValidationResult
        {
            Success = true
        };
    }

    public static TransactionValidationResult FromFail(string message)
    {
        return new TransactionValidationResult
        {
            Success = false,
            Message = message
        };
    }

    public bool Success { get; private set; }
    public string Message { get; private set; }
}
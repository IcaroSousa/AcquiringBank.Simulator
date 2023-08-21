using System.Collections.Generic;
using AcquiringBank.Data.Models.Payment;
using AcquiringBank.Domain.Card;
using AcquiringBank.Domain.Payment;

namespace AcquiringBank.Application.Services.Validators;

public class CardIsActiveValidator : IPaymentTransactionValidator
{
    public TransactionValidationResult ValidateTransaction(CardDto cardDetails,
        IEnumerable<TransactionModel> transactions,
        PaymentTransactionRequest transactionRequest)
    {
        return cardDetails.IsActive
            ? TransactionValidationResult.FromSuccess()
            : TransactionValidationResult.FromFail("Card is not active!");
    }
}
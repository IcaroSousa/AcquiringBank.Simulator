using System.Collections.Generic;
using System.Linq;
using AcquiringBank.Data.Models.Payment;
using AcquiringBank.Domain.Card;
using AcquiringBank.Domain.Payment;

namespace AcquiringBank.Application.Services.Validators;

public class CardHasEnoughLimitValidator : IPaymentTransactionValidator
{
    public TransactionValidationResult ValidateTransaction(CardDto cardDetails,
        IEnumerable<TransactionModel> transactions, PaymentTransactionRequest transactionRequest)
    {
        var expenses = transactions.Sum(px => px.Amount);

        return cardDetails.Limit <= expenses + transactionRequest.Details.Amount
            ? TransactionValidationResult.FromFail("Card has no limit to approve this transaction!")
            : TransactionValidationResult.FromSuccess();
    }
}
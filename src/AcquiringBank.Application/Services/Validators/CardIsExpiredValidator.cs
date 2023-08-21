using System;
using System.Collections.Generic;
using AcquiringBank.Data.Models.Payment;
using AcquiringBank.Domain.Card;
using AcquiringBank.Domain.Payment;

namespace AcquiringBank.Application.Services.Validators;

public class CardIsExpiredValidator : IPaymentTransactionValidator
{
    public TransactionValidationResult ValidateTransaction(CardDto cardDetails,
        IEnumerable<TransactionModel> transactions,
        PaymentTransactionRequest transactionRequest)
    {
        var submittedAt = DateOnly.FromDateTime(transactionRequest.Details.SubmittedAt);
        var expiresAt = new DateOnly(cardDetails.ExpiresAt.Year, cardDetails.ExpiresAt.Month,
            DateTime.DaysInMonth(cardDetails.ExpiresAt.Year, cardDetails.ExpiresAt.Month));

        return submittedAt <= expiresAt
            ? TransactionValidationResult.FromSuccess()
            : TransactionValidationResult.FromFail("Card is expired!");
    }
}
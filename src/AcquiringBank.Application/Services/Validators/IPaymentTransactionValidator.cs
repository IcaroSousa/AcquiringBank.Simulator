using System.Collections.Generic;
using AcquiringBank.Data.Models.Payment;
using AcquiringBank.Domain.Card;
using AcquiringBank.Domain.Payment;

namespace AcquiringBank.Application.Services.Validators;

public interface IPaymentTransactionValidator
{
    TransactionValidationResult ValidateTransaction(CardDto cardDetails, IEnumerable<TransactionModel> transactions,
        PaymentTransactionRequest transactionRequest);
}